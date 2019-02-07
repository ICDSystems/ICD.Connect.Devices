using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using ICD.Common.Properties;
using ICD.Common.Utils;
using ICD.Common.Utils.Collections;
using ICD.Common.Utils.Comparers;
using ICD.Common.Utils.Extensions;
using ICD.Connect.API.Nodes;

namespace ICD.Connect.Devices.Controls
{
	public sealed class DeviceControlsCollection : IEnumerable<IDeviceControl>, IStateDisposable, IApiNodeGroup
	{
		private readonly Dictionary<Type, List<IDeviceControl>> m_TypeToControls;
		private readonly IcdOrderedDictionary<int, IDeviceControl> m_DeviceControls;
		private readonly SafeCriticalSection m_DeviceControlsSection;

		private static readonly PredicateComparer<IDeviceControl, int> s_ControlComparer;

		#region Properties

		/// <summary>
		/// Gets the number of controls in the collection.
		/// </summary>
		public int Count { get { return m_DeviceControlsSection.Execute(() => m_DeviceControls.Count); } }

		/// <summary>
		/// Returns true if this instance has been disposed.
		/// </summary>
		public bool IsDisposed { get; private set; }

		#endregion

		/// <summary>
		/// Static constructor.
		/// </summary>
		static DeviceControlsCollection()
		{
			s_ControlComparer = new PredicateComparer<IDeviceControl, int>(c => c.Id);
		}

		/// <summary>
		/// Constructor.
		/// </summary>
		public DeviceControlsCollection()
		{
			m_TypeToControls = new Dictionary<Type, List<IDeviceControl>>();
			m_DeviceControls = new IcdOrderedDictionary<int, IDeviceControl>();
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
			if (item == null)
				throw new ArgumentNullException("item");

			m_DeviceControlsSection.Enter();

			try
			{
				IDeviceControl existing;
				if (m_DeviceControls.TryGetValue(item.Id, out existing))
				{
					string message = string.Format("Failed to add {0} - already contains a {1} with id {2}",
					                               item.GetType().Name, existing.GetType().Name, item.Id);
					throw new InvalidOperationException(message);
				}

				m_DeviceControls.Add(item.Id, item);

				foreach (Type type in item.GetType().GetAllTypes())
				{
					List<IDeviceControl> controls;
					if (!m_TypeToControls.TryGetValue(type, out controls))
					{
						controls = new List<IDeviceControl>();
						m_TypeToControls[type] = controls;
					}

					controls.AddSorted(item, s_ControlComparer);
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
				m_DeviceControls.Clear();
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
				IDeviceControl control;
				if (!m_DeviceControls.TryGetValue(id, out control))
					return false;

				m_DeviceControls.Remove(id);

				foreach (List<IDeviceControl> group in m_TypeToControls.Values)
					group.Remove(control);
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
			where T : class, IDeviceControl
		{
			m_DeviceControlsSection.Enter();

			try
			{
				List<IDeviceControl> controls;
				if (!m_TypeToControls.TryGetValue(typeof(T), out controls))
					return null;

				return controls.Count == 0 ? null : controls[0] as T;
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
			where T : class, IDeviceControl
		{
			// Edge case - we use control id 0 as a lookup
			IDeviceControl control = id == 0 ? GetControl<T>() : null;
			control = control ?? GetControl(id);

			if (control == null)
				throw new ArgumentException(string.Format("No control of type {0}", typeof(T).Name), "id");

			T output = control as T;
			if (output != null)
				return output;

			string message = string.Format("{0} is not of type {1}", control, typeof(T).Name);
			throw new InvalidOperationException(message);
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
				IDeviceControl control;
				if (m_DeviceControls.TryGetValue(id, out control))
					return control;

				throw new KeyNotFoundException(string.Format("{0} does not contain control with id {1}", GetType().Name, id));
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
			where T : class, IDeviceControl
		{
			m_DeviceControlsSection.Enter();

			try
			{
				Type type = typeof(T);

				List<IDeviceControl> controls;
				return m_TypeToControls.TryGetValue(type, out controls)
					? controls.Cast<T>().ToArray(controls.Count)
					: Enumerable.Empty<T>();
			}
			finally
			{
				m_DeviceControlsSection.Leave();
			}
		}

		public IEnumerator<IDeviceControl> GetEnumerator()
		{
			m_DeviceControlsSection.Enter();

			try
			{
				return m_DeviceControls.Values
				                       .ToList(m_DeviceControls.Count)
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
