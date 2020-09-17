using ICD.Connect.API.EventArguments;
using ICD.Connect.Devices.Controls.Power;
using ICD.Connect.Devices.Proxies.Controls;

namespace ICD.Connect.Devices.EventArguments
{
	public sealed class PowerDeviceControlPowerStateApiEventArgs : AbstractGenericApiEventArgs<PowerDeviceControlPowerStateEventData>
	{
		/// <summary>
		/// </summary>
		/// <param name="data"></param>
		public PowerDeviceControlPowerStateApiEventArgs(PowerDeviceControlPowerStateEventData data)
			: base(PowerDeviceControlApi.EVENT_POWER_STATE, data)
		{
		}

		/// <summary>
		/// </summary>
		/// <param name="powerState"></param>
		/// <param name="expectedDuration"></param>
		public PowerDeviceControlPowerStateApiEventArgs(ePowerState powerState, long expectedDuration)
			: this(new PowerDeviceControlPowerStateEventData(powerState, expectedDuration))
		{
		}

		/// <summary>
		/// </summary>
		/// <param name="powerState"></param>
		public PowerDeviceControlPowerStateApiEventArgs(ePowerState powerState) 
			: this(powerState, 0)
		{
		}
	}
}
