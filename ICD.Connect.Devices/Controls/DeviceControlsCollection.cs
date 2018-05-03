using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using ICD.Common.Properties;
using ICD.Common.Utils;
using ICD.Common.Utils.Extensions;
using ICD.Connect.API.Nodes;

namespace ICD.Connect.Devices.Controls
{
	public sealed class DeviceControlsCollection : IEnumerable<IDeviceControl>, IStateDisposable, IApiNodeGroup
	{
		private readonly Dictionary<Type, List<int>> m_TypeToControls;
		private readonly List<int> m_OrderedControls;
		private readonly Dictionary<int, IDeviceControl> m_DeviceControls;
		private readonly SafeCriticalSection m_DeviceControlsSection;

		#region Properties

		public int Count { get { return m_DeviceControlsSection.Execute(() => m_DeviceControls.Count); } }

		/// <summary>
		/// Returns true if this instance has been disposed.
		/// </summary>
		public bool IsDisposed { get; private set; }

		#endregion

		/// <summary>
		/// Constructor.
		/// </summary>
		public DeviceControlsCollection()
		{
			m_TypeToControls = new Dictionary<Type, List<int>>();
			m_OrderedControls = new List<int>();
			m_DeviceControls = new Dictionary<int, IDeviceControl>();
			m_DeviceControlsSection = new SafeCriticalSection();
		}

		#region Methods

		/// <summary>
		/// Release resources.
		/// </summary>
		public void Dispose()
		{
			Dispose(true);
		}

		/// <summary>
		/// Adds the control to the collection.
		/// </summary>
		/// <param name="item"></param>
		public void Add(IDeviceControl item)
		{
			m_DeviceControlsSection.Enter();

			try
			{
				m_DeviceControls.Add(item.Id, item);
				m_OrderedControls.AddSorted(item.Id);

				foreach (Type type in item.GetType().GetAllTypes())
				{
					if (!m_TypeToControls.ContainsKey(type))
						m_TypeToControls[type] = new List<int>();
					m_TypeToControls[type].AddSorted(item.Id);
				}
			}
			finally
			{
				m_DeviceControlsSection.Leave();
			}
		}

		/// <summary>
		/// Removes all controls from the collection.
		/// </summary>
		public void Clear()
		{
			m_DeviceControlsSection.Enter();

			try
			{
				m_TypeToControls.Clear();
				m_OrderedControls.Clear();
				m_DeviceControls.Clear();
			}
			finally
			{
				m_DeviceControlsSection.Leave();
			}
		}

		/// <summary>
		/// Gets the control with the given id.
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		[NotNull]
		public IDeviceControl GetControl(int id)
		{
			m_DeviceControlsSection.Enter();

			try
			{
				if (!m_DeviceControls.ContainsKey(id))
					throw new KeyNotFoundException(string.Format("{0} does not contain control with id {1}", GetType().Name, id));
				return m_DeviceControls[id];
			}
			finally
			{
				m_DeviceControlsSection.Leave();
			}
		}

		/// <summary>
		/// Attempts to get the control with the given id.
		/// </summary>
		/// <param name="id"></param>
		/// <param name="control"></param>
		/// <returns></returns>
		public bool TryGetControl(int id, out IDeviceControl control)
		{
			m_DeviceControlsSection.Enter();

			try
			{
				return m_DeviceControls.TryGetValue(id, out control);
			}
			finally
			{
				m_DeviceControlsSection.Leave();
			}
		}

		/// <summary>
		/// Removes the control with the given id from the collection.
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		public bool Remove(int id)
		{
			m_DeviceControlsSection.Enter();

			try
			{
				bool output = m_DeviceControls.Remove(id);
				if (!output)
					return false;

				m_OrderedControls.Remove(id);

				foreach (List<int> group in m_TypeToControls.Values)
					group.Remove(id);
			}
			finally
			{
				m_DeviceControlsSection.Leave();
			}

			return true;
		}

		/// <summary>
		/// Returns true if the collection contains a control with the given id.
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		public bool Contains(int id)
		{
			return m_DeviceControlsSection.Execute(() => m_DeviceControls.ContainsKey(id));
		}

		/// <summary>
		/// Gets the first control of the given type.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <returns></returns>
		[CanBeNull]
		public T GetControl<T>()
			where T : IDeviceControl
		{
			m_DeviceControlsSection.Enter();

			try
			{
				List<int> ids;
				if (!m_TypeToControls.TryGetValue(typeof(T), out ids))
					return default(T);

				return ids.Count == 0 ? default(T) : GetControl<T>(ids.First());
			}
			finally
			{
				m_DeviceControlsSection.Leave();
			}
		}

		/// <summary>
		/// Gets the controls.
		/// </summary>
		/// <returns></returns>
		public IEnumerable<IDeviceControl> GetControls()
		{
			return GetControls<IDeviceControl>();
		}

		/// <summary>
		/// Gets the controls of the given type.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <returns></returns>
		public IEnumerable<T> GetControls<T>()
			where T : IDeviceControl
		{
			m_DeviceControlsSection.Enter();

			try
			{
				Type type = typeof(T);

				List<int> ids;
				if (!m_TypeToControls.TryGetValue(type, out ids))
					return Enumerable.Empty<T>();

				return ids.Select(id => GetControl(id))
				          .Cast<T>()
				          .ToArray();
			}
			finally
			{
				m_DeviceControlsSection.Leave();
			}
		}

		/// <summary>
		/// Gets the control with the given id and type.
		/// 
		/// Special case - If id is 0 we look up the first control of the given type.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="id"></param>
		/// <exception cref="InvalidOperationException">Throws InvalidOperationException if the given control does not exist.</exception>
		/// <returns></returns>
		[NotNull]
		public T GetControl<T>(int id)
			where T : IDeviceControl
		{
			IDeviceControl control;

			// Edge case - we use control id 0 as a lookup
			if (id == 0 && !Contains(id))
				control = GetControl<T>();
			else
				control = GetControl(id);

			if (control is T)
				return (T)control;

			string message = string.Format("{0} is not of type {1}", control, typeof(T).Name);
			throw new InvalidOperationException(message);
		}

		public IEnumerator<IDeviceControl> GetEnumerator()
		{
			m_DeviceControlsSection.Enter();

			try
			{
				return m_OrderedControls.Select(c => m_DeviceControls[c])
				                        .ToList()
				                        .GetEnumerator();
			}
			finally
			{
				m_DeviceControlsSection.Leave();
			}
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}

		#endregion

		#region Private Methods

		/// <summary>
		/// Releases resources but also allows for finalizing without touching managed resources.
		/// </summary>
		/// <param name="disposing"></param>
		private void Dispose(bool disposing)
		{
			if (!IsDisposed)
				DisposeFinal(disposing);
			IsDisposed = IsDisposed || disposing;
		}

		/// <summary>
		/// Override to release resources.
		/// </summary>
		/// <param name="disposing"></param>
		private void DisposeFinal(bool disposing)
		{
			if (!disposing)
				return;

			m_DeviceControlsSection.Enter();

			try
			{
				foreach (IDeviceControl control in m_DeviceControls.Values)
					control.Dispose();
				Clear();
			}
			finally
			{
				m_DeviceControlsSection.Leave();
			}
		}

		#endregion

		#region API Node Group

		/// <summary>
		/// Gets a sequence of keyed nodes.
		/// </summary>
		/// <returns></returns>
		IEnumerable<KeyValuePair<uint, object>> IApiNodeGroup.GetKeyedNodes()
		{
			return GetControls().Select(c => new KeyValuePair<uint, object>((uint)c.Id, c));
		}

		/// <summary>
		/// Gets the instance for the given key.
		/// </summary>
		/// <param name="key"></param>
		/// <returns></returns>
		object IApiNodeGroup.this[uint key] { get { return GetControl((int)key); } }

		/// <summary>
		/// Returns true if the group contains an instance for the given key.
		/// </summary>
		/// <param name="key"></param>
		/// <returns></returns>
		bool IApiNodeGroup.ContainsKey(uint key)
		{
			return Contains((int)key);
		}

		#endregion
	}
}
