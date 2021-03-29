using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using ICD.Common.Properties;
using ICD.Common.Utils;
using ICD.Common.Utils.Collections;
using ICD.Common.Utils.Extensions;

namespace ICD.Connect.Devices.Telemetry.DeviceInfo
{
	public sealed class AdapterNetworkDeviceInfoCollection : IEnumerable<IAdapterNetworkDeviceInfo>, INotifyCollectionChanged
	{
		public event EventHandler OnCollectionChanged;

		private readonly SafeCriticalSection m_AdapterSection;
		private readonly IcdSortedDictionary<int, IAdapterNetworkDeviceInfo> m_Adapters;
		private readonly Func<int, IAdapterNetworkDeviceInfo> m_CreateAdapter;

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="createAdapter"></param>
		public AdapterNetworkDeviceInfoCollection(Func<int, IAdapterNetworkDeviceInfo> createAdapter)
		{
			m_AdapterSection = new SafeCriticalSection();
			m_Adapters = new IcdSortedDictionary<int, IAdapterNetworkDeviceInfo>();
			m_CreateAdapter = createAdapter;
		}

		#region Methods

		public void Clear()
		{
			m_AdapterSection.Execute(() => m_Adapters.Clear());

			OnCollectionChanged.Raise(this);
		}

		[NotNull]
		public IAdapterNetworkDeviceInfo GetOrAddAdapter(int address)
		{
			IAdapterNetworkDeviceInfo adapter;

			m_AdapterSection.Enter();

			try
			{
				if (m_Adapters.TryGetValue(address, out adapter))
					return adapter;

				adapter = m_CreateAdapter(address);
				m_Adapters.Add(address, adapter);
			}
			finally
			{
				m_AdapterSection.Leave();
			}

			OnCollectionChanged.Raise(this);

			return adapter;
		}

		#endregion

		#region IEnumerable

		public IEnumerator<IAdapterNetworkDeviceInfo> GetEnumerator()
		{
			return m_AdapterSection.Execute(() => m_Adapters.Values.ToList()).GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}

		#endregion
	}
}