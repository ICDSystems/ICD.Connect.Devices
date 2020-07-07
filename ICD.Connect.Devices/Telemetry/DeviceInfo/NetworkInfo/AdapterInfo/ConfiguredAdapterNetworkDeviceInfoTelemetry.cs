using System;

namespace ICD.Connect.Devices.Telemetry.DeviceInfo.NetworkInfo.AdapterInfo
{
	public sealed class ConfiguredAdapterNetworkDeviceInfoTelemetry : AbstractAdapterNetworkDeviceInfoTelemetry, IConfiguredAdapterNetworkDeviceInfoTelemetry
	{


		public ConfiguredAdapterNetworkDeviceInfoTelemetry(int address) : base(address)
		{
		}

		/// <summary>
		/// Apply the configuration from the settings
		/// </summary>
		/// <param name="settings"></param>
		public void ApplySettings(ConfiguredAdapterNetworkDeviceInfoTelemetrySettings settings)
		{
			if (Address != settings.Address)
				throw new InvalidOperationException("Cannot Apply Settings with different Addresses");

			Name = settings.Name;
			MacAddress = settings.MacAddress;
			Dhcp = settings.Dhcp;
			Ipv4Address = settings.Ipv4Address;
			Ipv4SubnetMask = settings.Ipv4SubnetMask;
			Ipv4Gateway = settings.Ipv4Gateway;
		}

		/// <summary>
		/// Copy the configuration to the given settings
		/// </summary>
		/// <param name="settings"></param>
		public void CopySettings(ConfiguredAdapterNetworkDeviceInfoTelemetrySettings settings)
		{
			settings.Address = Address;
			settings.Name = Name;
			settings.MacAddress = MacAddress;
			settings.Dhcp = Dhcp;
			settings.Ipv4Address = Ipv4Address;
			settings.Ipv4SubnetMask = Ipv4SubnetMask;
			settings.Ipv4Gateway = Ipv4Gateway;
		}

		/// <summary>
		/// Clear the items from settings
		/// </summary>
		public void ClearSettings()
		{
			Name = null;
			MacAddress = null;
			Dhcp = null;
			Ipv4Address = null;
			Ipv4SubnetMask = null;
			Ipv4Gateway = null;
		}
	}
}