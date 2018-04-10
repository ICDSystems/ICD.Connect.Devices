using ICD.Connect.API.EventArguments;
using ICD.Connect.Devices.Proxies.Controls;

namespace ICD.Connect.Devices.EventArguments
{
	public sealed class PowerDeviceControlPowerStateApiEventArgs : AbstractGenericApiEventArgs<bool>
	{
		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="data"></param>
		public PowerDeviceControlPowerStateApiEventArgs(bool data)
			: base(PowerDeviceControlApi.EVENT_IS_POWERED, data)
		{
		}
	}
}
