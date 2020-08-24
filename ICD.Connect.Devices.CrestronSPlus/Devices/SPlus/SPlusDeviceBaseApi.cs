namespace ICD.Connect.Devices.CrestronSPlus.Devices.SPlus
{
	public static class SPlusDeviceBaseApi
	{
		#region Const's

		public const string METHOD_SET_IS_ONLINE = "SetIsOnline";

		public const string HELP_METHOD_SET_IS_ONLINE = "Sets the online state of the device from the shim";

		public const string EVENT_ON_REQUEST_SHIM_RESYNC = "OnRequestShimResync";

		public const string EVENT_ON_REQUEST_SHIM_RESYNC_HELP = "Sent by the originator to request the shim to resync";

		#endregion
	}
}