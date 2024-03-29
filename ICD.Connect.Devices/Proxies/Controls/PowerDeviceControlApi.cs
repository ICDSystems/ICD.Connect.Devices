﻿namespace ICD.Connect.Devices.Proxies.Controls
{
	public static class PowerDeviceControlApi
	{
		public const string EVENT_POWER_STATE = "OnPowerStateChanged";
		public const string PROPERTY_POWER_STATE = "PowerState";

		public const string METHOD_POWER_ON = "PowerOn";
		public const string METHOD_POWER_ON_BYPASS = "PowerOnBypass";
		public const string METHOD_POWER_OFF = "PowerOff";
		public const string METHOD_POWER_OFF_BYPASS = "PowerOffBypass";

		public const string HELP_EVENT_POWER_STATE = "Raised when the powered state changes.";
		public const string HELP_PROPERTY_POWER_STATE = "The powered state of the device.";

		public const string HELP_METHOD_POWER_ON = "Powers on the device.";
		public const string HELP_METHOD_POWER_ON_BYPASS = "Powers on the device. If parameter is true, ignores the pre power on delegate.";
		public const string HELP_METHOD_POWER_OFF = "Powers off the device.";
		public const string HELP_METHOD_POWER_OFF_BYPASS = "Powers off the device. If parameter is true, ignores the post power off delegate";
	}
}
