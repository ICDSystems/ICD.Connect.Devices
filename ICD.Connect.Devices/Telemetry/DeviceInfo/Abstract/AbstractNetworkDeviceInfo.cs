using System;
using ICD.Common.Utils.EventArguments;
using ICD.Common.Utils.Extensions;

namespace ICD.Connect.Devices.Telemetry.DeviceInfo.Abstract
{
	public abstract class AbstractNetworkDeviceInfo : INetworkDeviceInfo
	{
		#region Events

		public event EventHandler<StringEventArgs> OnHostnameChanged;
		public event EventHandler<StringEventArgs> OnDnsChanged;

		#endregion

		#region Fields

		private string m_Hostname;
		private string m_Dns;
		private readonly AdapterNetworkDeviceInfoCollection m_Adapters;

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

		public AdapterNetworkDeviceInfoCollection Adapters { get { return m_Adapters; } }

		#endregion

		#region Constructor

		/// <summary>
		/// Constructor.
		/// </summary>
		protected AbstractNetworkDeviceInfo()
		{
			m_Adapters = new AdapterNetworkDeviceInfoCollection(CreateNewAdapter);
		}

		#endregion

		#region Methods

		public void Clear()
		{
			Hostname = null;
			Dns = null;
			Adapters.Clear();
		}

		/// <summary>
		/// Initializes the current telemetry state.
		/// </summary>
		public void InitializeTelemetry()
		{
		}

		#endregion

		protected abstract IAdapterNetworkDeviceInfo CreateNewAdapter(int address);
	}
}