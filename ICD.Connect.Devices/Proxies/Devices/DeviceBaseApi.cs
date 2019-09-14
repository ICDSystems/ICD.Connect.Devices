namespace ICD.Connect.Devices.Proxies.Devices
{
	public static class DeviceBaseApi
	{
		public const string EVENT_IS_ONLINE = "OnIsOnlineStateChanged";
		public const string EVENT_CONTROLS_AVALIABLE = "OnControlsAvaliableChanged";
		public const string PROPERTY_IS_ONLINE = "IsOnline";
		public const string PROPERTY_CONTROLS_AVALIABLE = "ControlsAvaliable";
		public const string NODE_GROUP_CONTROLS = "Controls";

		public const string HELP_EVENT_IS_ONLINE = "Raised when the online state changes.";
		public const string HELP_EVENT_CONTROLS_AVALIABLE = "Raised when controls available changes.";
		public const string HELP_PROPERTY_IS_ONLINE = "Gets the online state of the device.";
		public const string HELP_PROPERTY_CONTROLS_AVALIABLE = "Gets the current state of controls available";
		public const string HELP_NODE_GROUP_CONTROLS = "The controls for this device.";
	}
}
