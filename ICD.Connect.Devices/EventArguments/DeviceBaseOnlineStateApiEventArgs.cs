using ICD.Connect.API.EventArguments;
using ICD.Connect.Devices.Proxies.Devices;

namespace ICD.Connect.Devices.EventArguments
{
	public sealed class DeviceBaseOnlineStateApiEventArgs : AbstractGenericApiEventArgs<bool>
	{
		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="data"></param>
		public DeviceBaseOnlineStateApiEventArgs(bool data)
			: base(DeviceBaseApi.EVENT_IS_ONLINE, data)
		{
		}
	}
}
