using System;
using ICD.Common.Properties;
using ICD.Common.Utils.EventArguments;
using ICD.Connect.Telemetry.Providers;

namespace ICD.Connect.Devices.Telemetry.DeviceInfo.Abstract
{
	public abstract class AbstractAdapterNetworkDeviceInfo : IAdapterNetworkDeviceInfo
	{
		public event EventHandler<StringEventArgs> OnNameChanged;
		public event EventHandler<GenericEventArgs<IcdPhysicalAddress>> OnMacAddressChanged;
		public event EventHandler<GenericEventArgs<bool?>> OnDhcpChanged;
		public event EventHandler<StringEventArgs> OnIpv4AddressChanged;
		public event EventHandler<StringEventArgs> OnIpv4SubnetMaskChanged;
		public event EventHandler<StringEventArgs> OnIpv4GatewayChanged;

		private readonly int m_Address;

		private string m_Name;
		private IcdPhysicalAddress m_MacAddress;
		private bool? m_Dhcp;
		private string m_Ipv4Address;
		private string m_Ipv4SubnetMask;
		private string m_Ipv4Gateway;

		public int Address
		{
			get { return m_Address; }
		}

		public string Name
		{
			get { return m_Name; }
			set
			{
				if (m_Name == value)
					return;

				m_Name = value;

				OnNameChanged.Raise(this, m_Name);
			}
		}

		public IcdPhysicalAddress MacAddress
		{
			get { return m_MacAddress; }
			set
			{
				if (m_MacAddress == value)
					return;

				m_MacAddress = value;

				OnMacAddressChanged.Raise(this, m_MacAddress);
			}
		}

		[PublicAPI("DAV")]
		public string MacAddressString
		{
			get { return m_MacAddress == null ? null : m_MacAddress.ToString(); }
			set
			{
				IcdPhysicalAddress mac;
				IcdPhysicalAddress.TryParse(value, out mac);
				MacAddress = mac;
			}
		}

		public bool? Dhcp
		{
			get { return m_Dhcp; }
			set
			{
				if (m_Dhcp == value)
					return;

				m_Dhcp = value;

				OnDhcpChanged.Raise(this, m_Dhcp);
			}
		}

		public string Ipv4Address
		{
			get { return m_Ipv4Address; }
			set
			{
				if (m_Ipv4Address == value)
					return;

				m_Ipv4Address = value;

				OnIpv4AddressChanged.Raise(this, m_Ipv4Address);
			}
		}

		public string Ipv4SubnetMask
		{
			get { return m_Ipv4SubnetMask; }
			set
			{
				if (m_Ipv4SubnetMask == value)
					return;

				m_Ipv4SubnetMask = value;

				OnIpv4SubnetMaskChanged.Raise(this, m_Ipv4SubnetMask);
			}
		}

		public string Ipv4Gateway
		{
			get { return m_Ipv4Gateway; }
			set
			{
				if (m_Ipv4Gateway == value)
					return;

				m_Ipv4Gateway = value;

				OnIpv4GatewayChanged.Raise(this, m_Ipv4Gateway);
			}
		}

		protected AbstractAdapterNetworkDeviceInfo(int address)
		{
			m_Address = address;
		}

		/// <summary>
		/// Initializes the current telemetry state.
		/// </summary>
		void ITelemetryProvider.InitializeTelemetry()
		{
		}
	}
}