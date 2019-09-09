using ICD.Connect.API.EventArguments;
using ICD.Connect.Devices.Controls;
using ICD.Connect.Devices.Proxies.Controls;

namespace ICD.Connect.Devices.EventArguments
{
	public sealed class PowerDeviceControlPowerStateApiEventArgs : AbstractGenericApiEventArgs<ePowerState>
	{
		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="data"></param>
		public PowerDeviceControlPowerStateApiEventArgs(ePowerState data)
			: base(PowerDeviceControlApi.EVENT_POWER_STATE, data)
		{
		}
	}
}
