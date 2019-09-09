namespace ICD.Connect.Devices.Proxies.Controls
{
	public static class DeviceControlApi
	{
		public const string EVENT_CONTROL_AVALIABLE = "OnControlAvaliableChanged";
		public const string PROPERTY_ID = "Id";
		public const string PROPERTY_NAME = "Name";
		public const string PROPERTY_CONTROL_AVALIABLE = "ControlAvaliable";

		public const string HELP_EVENT_CONTROL_AVALIABLE = "Raised when the control avaliability changes";
		public const string HELP_PROPERTY_ID = "The unique ID of the control within the device.";
		public const string HELP_PROPERTY_NAME = "The name of the control.";
		public const string HELP_PROPERTY_CONTROL_AVALIABLE = "If the control is avaliable or not";
	}
}
