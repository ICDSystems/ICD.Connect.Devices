using ICD.Connect.API.EventArguments;
using ICD.Connect.Devices.Proxies.Devices;

namespace ICD.Connect.Devices.EventArguments
{
	public sealed class DeviceBaseControlsAvailableApiEventArgs : AbstractGenericApiEventArgs<bool>
	{
		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="data"></param>
		public DeviceBaseControlsAvailableApiEventArgs(bool data)
			: base(DeviceBaseApi.EVENT_CONTROLS_AVAILABLE, data)
		{
		}
	}
}
