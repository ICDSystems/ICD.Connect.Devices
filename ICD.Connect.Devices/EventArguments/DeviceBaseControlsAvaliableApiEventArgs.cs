using ICD.Connect.API.EventArguments;
using ICD.Connect.Devices.Proxies.Devices;

namespace ICD.Connect.Devices.EventArguments
{
    public sealed class DeviceBaseControlsAvaliableApiEventArgs : AbstractGenericApiEventArgs<bool>
	{
		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="data"></param>
		public DeviceBaseControlsAvaliableApiEventArgs(bool data) : base(DeviceBaseApi.EVENT_CONTROLS_AVALIABLE, data)
		{
		}
	}
}
