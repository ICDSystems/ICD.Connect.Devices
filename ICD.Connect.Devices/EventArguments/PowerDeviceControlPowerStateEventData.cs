using System;
using ICD.Connect.Devices.Controls;
using ICD.Connect.Devices.Controls.Power;
using Newtonsoft.Json;

namespace ICD.Connect.Devices.EventArguments
{
	[Serializable]
	public sealed class PowerDeviceControlPowerStateEventData
	{
		public ePowerState PowerState { get; set; }

		public long ExpectedDuration { get; set; }

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="powerState"></param>
		public PowerDeviceControlPowerStateEventData(ePowerState powerState)
			: this(powerState, 0)
		{
		}

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="powerState"></param>
		/// <param name="expectedDuration"></param>
		[JsonConstructor]
		public PowerDeviceControlPowerStateEventData(ePowerState powerState, long expectedDuration)
		{
			PowerState = powerState;
			ExpectedDuration = expectedDuration;
		}
	}
}
