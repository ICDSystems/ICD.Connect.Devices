using ICD.Connect.API.EventArguments;
using ICD.Connect.Devices.Proxies.Controls;

namespace ICD.Connect.Devices.EventArguments
{
	public sealed class DeviceControlAvailableApiEventArgs : AbstractGenericApiEventArgs<bool>
	{
		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="data"></param>
		public DeviceControlAvailableApiEventArgs(bool data)
			: base(DeviceControlApi.EVENT_CONTROL_AVAILABLE, data)
		{
		}
	}
}
