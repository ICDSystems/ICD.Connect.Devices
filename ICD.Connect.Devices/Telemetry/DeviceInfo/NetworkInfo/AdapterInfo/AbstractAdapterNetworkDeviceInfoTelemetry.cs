using System;
using ICD.Common.Utils.EventArguments;
using ICD.Common.Utils.Extensions;

namespace ICD.Connect.Devices.Telemetry.DeviceInfo.NetworkInfo.AdapterInfo
{
	public abstract class AbstractAdapterNetworkDeviceInfoTelemetry : IAdapterNetworkDeviceInfoTelemetry
	{
		private readonly int m_Address;
		private string m_Name;
		private string m_MacAddress;
		private bool? m_Dhcp;
		private string m_Ipv4Address;
		private string m_Ipv4SubnetMask;
		private string m_Ipv4Gateway;
		public event EventHandler<StringEventArgs> OnNameChanged;
		public event EventHandler<StringEventArgs> OnMacAddressChanged;
		public event EventHandler<GenericEventArgs<bool?>> OnDhcpChanged;
		public event EventHandler<StringEventArgs> OnIpv4AddressChanged;
		public event EventHandler<StringEventArgs> OnIpv4SubnetMaskChanged;
		public event EventHandler<StringEventArgs> OnIpv4GatewayChanged;


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

				OnNameChanged.Raise(this, new StringEventArgs(value));
			}
		}

		public string MacAddress
		{
			get { return m_MacAddress; }
			set
			{
				if (m_MacAddress == value)
					return;

				m_MacAddress = value;

				OnMacAddressChanged.Raise(this, new StringEventArgs(value));
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

				OnDhcpChanged.Raise(this, new GenericEventArgs<bool?>(value));
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

				OnIpv4AddressChanged.Raise(this, new StringEventArgs(value));
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

				OnIpv4SubnetMaskChanged.Raise(this, new StringEventArgs(value));
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

				OnIpv4GatewayChanged.Raise(this, new StringEventArgs(value));
			}
		}

		protected AbstractAdapterNetworkDeviceInfoTelemetry(int address)
		{
			m_Address = address;
		}
	}
}