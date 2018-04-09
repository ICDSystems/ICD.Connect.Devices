namespace ICD.Connect.Devices.Proxies.Controls
{
	public static class PowerDeviceControlApi
	{
		public const string EVENT_IS_POWERED = "OnIsPoweredChanged";
		public const string PROPERTY_IS_POWERED = "IsPowered";

		public const string METHOD_POWER_ON = "PowerOn";
		public const string METHOD_POWER_OFF = "PowerOff";

		public const string HELP_EVENT_IS_POWERED = "Raised when the powered state changes.";
		public const string HELP_PROPERTY_IS_POWERED = "The powered state of the device.";

		public const string HELP_METHOD_POWER_ON = "Powers on the device.";
		public const string HELP_METHOD_POWER_OFF = "Powers off the device.";
	}
}
