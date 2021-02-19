using System;
using System.Collections.Generic;
using System.Linq;
using ICD.Common.Utils;
using ICD.Common.Utils.EventArguments;
using ICD.Common.Utils.Extensions;

namespace ICD.Connect.Devices.Telemetry.DeviceInfo.Abstract
{
	public abstract class AbstractNetworkDeviceInfo<TAdapterInfo> : INetworkDeviceInfo<TAdapterInfo>
		where TAdapterInfo : IAdapterNetworkDeviceInfo
	{
		#region Fields

		private string m_Hostname;
		private string m_Dns;
		private readonly SafeCriticalSection m_AdapterSection;
		private readonly Dictionary<int, TAdapterInfo> m_Adapters;

		#endregion

		#region Events

		public event EventHandler<StringEventArgs> OnHostnameChanged;
		public event EventHandler<StringEventArgs> OnDnsChanged;
		public event EventHandler OnAdaptersChanged;

		#endregion

		#region Properties

		public string Hostname
		{
			get { return m_Hostname; }
			set
			{
				if (value == m_Hostname)
					return;

				m_Hostname = value;

				OnHostnameChanged.Raise(this, new StringEventArgs(value));
			}
		}

		public string Dns
		{
			get { return m_Dns; }
			set
			{
				if (value == m_Dns)
					return;

				m_Dns = value;

				OnDnsChanged.Raise(this, new StringEventArgs(value));
			}
		}

		IEnumerable<IAdapterNetworkDeviceInfo> INetworkDeviceInfo.Adapters
		{
			get { return m_AdapterSection.Execute(() => Adapters.Cast<IAdapterNetworkDeviceInfo>()); }
		}

		public IEnumerable<TAdapterInfo> Adapters
		{
			get { return m_AdapterSection.Execute(() => m_Adapters.Values.ToList(m_Adapters.Count)); }
		}

		#endregion

		#region Constructor

		/// <summary>
		/// Constructor.
		/// </summary>
		protected AbstractNetworkDeviceInfo()
		{
			m_AdapterSection = new SafeCriticalSection();
			m_Adapters = new Dictionary<int, TAdapterInfo>();
		}

		#endregion

		#region Methods

		public TAdapterInfo GetOrAddAdapter(int address)
		{
			m_AdapterSection.Enter();

			try
			{
				TAdapterInfo adapter;
				if (!m_Adapters.TryGetValue(address, out adapter))
				{
					adapter = CreateNewAdapter(address);
					m_Adapters.Add(address, adapter);
					OnAdaptersChanged.Raise(this);
				}
				return adapter;
			}
			finally
			{
				m_AdapterSection.Leave();
			}
		}

		IAdapterNetworkDeviceInfo INetworkDeviceInfo.GetOrAddAdapter(int address)
		{
			return GetOrAddAdapter(address);
		}

		protected abstract TAdapterInfo CreateNewAdapter(int address);

		protected void AdaptersAddRange(IEnumerable<TAdapterInfo> adapters)
		{
			m_AdapterSection.Execute(() => m_Adapters.AddRange(adapters, a => a.Address));
		}

		protected void AdaptersClear()
		{
			m_AdapterSection.Execute(() => m_Adapters.Clear());
		}

		public virtual void Clear()
		{
			Hostname = null;
			Dns = null;
			AdaptersClear();
		}

		/// <summary>
		/// Initializes the current telemetry state.
		/// </summary>
		public void InitializeTelemetry()
		{
		}

		#endregion
	}
}