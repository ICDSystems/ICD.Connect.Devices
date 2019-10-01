using System;
using ICD.Connect.Devices.Controls;

namespace ICD.Connect.Devices.EventArguments
{
	[Serializable]
	public sealed class PowerDeviceControlPowerStateEventData
	{
		public ePowerState PowerState { get; set; }

		public long ExpectedDuration { get; set; }

		public PowerDeviceControlPowerStateEventData(ePowerState powerState, long expectedDuration)
		{
			PowerState = powerState;
			ExpectedDuration = expectedDuration;
		}

		public PowerDeviceControlPowerStateEventData(ePowerState powerState)
		{
			PowerState = powerState;
			ExpectedDuration = 0;
		}
	}
}
