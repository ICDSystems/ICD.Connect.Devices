namespace ICD.Connect.Devices.Telemetry
{
	public static class DeviceTelemetryNames
	{
		public const string POWER_STATE = "IsPowered";
		public const string POWER_ON = "PowerOn";
		public const string POWER_OFF = "PowerOff";
		public const string POWER_STATE_CHANGED = "OnPowerStateChanged";

		public const string ONLINE_STATE = "IsOnline";
		public const string ONLINE_STATE_CHANGED = "OnIsOnlineChanged";

		public const string DEVICE_MAKE = "Make";
		public const string DEVICE_MAKE_CHANGED = "Make Changed";

		public const string DEVICE_MODEL = "Model";
		public const string DEVICE_MODEL_CHANGED = "Model Changed";

		public const string DEVICE_FIRMWARE_VERSION = "Firmware Version";
		public const string DEVICE_FIRMWARE_VERSION_CHANGED = "Firmware Version Changed";
		
		public const string DEVICE_FIRMWARE_DATE = "Firmware Date";
		public const string DEVICE_FIRMWARE_DATE_CHANGED = "Firmware Date Changed";

		public const string DEVICE_PURCHASE_DATE = "PurchaseDate";
		public const string DEVICE_PURCHASE_DATE_CHANGED = "PurchaseDate Changed";

		public const string DEVICE_HOSTNAME = "Hostname";
		public const string DEVICE_HOSTNAME_CHANGED = "Hostname Changed";

		public const string DEVICE_NETWORK_DNS = "Dns";
		public const string DEVICE_NETWORK_DNS_CHANGED = "Dns Changed";

		public const string DEVICE_NETWORK_ADAPTER_ADDRESS = "Address";

		public const string DEVICE_NETWORK_ADAPTER_NAME = "Name";
		public const string DEVICE_NETWORK_ADAPTER_NAME_CHANGED = "Name Changed";

		public const string DEVICE_MAC_ADDRESS = "MAC Address";
		public const string DEVICE_MAC_ADDRESS_CHANGED = "MAC Address Changed";

		public const string DEVICE_IP_ADDRESS = "IPv4 Address";
		public const string DEVICE_IP_ADDRESS_CHANGED = "IPv4 Address Changed";

		public const string DEVICE_IP_SUBNET = "IPv4 Subnet";
		public const string DEVICE_IP_SUBNET_CHANGED = "IPv4 Subnet Changed";

		public const string DEVICE_IP_GATEWAY = "IPv4 Gateway";
		public const string DEVICE_IP_GATEWAY_CHANGED = "IPv4 Gateway Changed";

		public const string DEVICE_DHCP_STATUS = "DHCP Enabled";
		public const string DEVICE_DHCP_STATUS_CHANGED = "DHCP Enabled Changed";

		public const string DEVICE_SERIAL_NUMBER = "Serial Number";
		public const string DEVICE_SERIAL_NUMBER_CHANGED = "Serial Number Changed";

		public const string DEVICE_UPTIME = "Uptime";
		public const string DEVICE_UPTIME_CHANGED = "Uptime Changed";
	}
}