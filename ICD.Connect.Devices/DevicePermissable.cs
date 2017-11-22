using ICD.Connect.Settings;

namespace ICD.Connect.Devices
{
	public class DevicePermissable : OriginatorPermissable
	{
		protected DevicePermissable(string val) : base(val)
		{
		}

		public static DevicePermissable ViewControls { get { return new DevicePermissable("DeviceAction.ViewControls"); } }
	}
}
