using System;
using System.Collections.Generic;
using System.Text;
using ICD.Connect.Devices.Controls;

namespace ICD.Connect.Devices.EventArguments
{
	public sealed class PowerDeviceControlPowerStateEventData
	{

		public ePowerState PowerState { get; set; }

		public int ExpectedDuration { get; set; }

		public PowerDeviceControlPowerStateEventData(ePowerState powerState, int expectedDuration)
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
