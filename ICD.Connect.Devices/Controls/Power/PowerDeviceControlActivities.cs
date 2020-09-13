using System;
using ICD.Common.Logging.Activities;
using ICD.Common.Utils.Services.Logging;

namespace ICD.Connect.Devices.Controls.Power
{
	public static class PowerDeviceControlActivities
	{
		public static Activity GetPowerActivity(ePowerState powerState)
		{
			switch (powerState)
			{
				case ePowerState.Unknown:
					return new Activity(Activity.ePriority.Medium, "Powered", "Unknown Power State", eSeverity.Warning);
				case ePowerState.PowerOff:
					return new Activity(Activity.ePriority.Medium, "Powered", "Powered Off", eSeverity.Informational);
				case ePowerState.PowerOn:
					return new Activity(Activity.ePriority.Medium, "Powered", "Powered On", eSeverity.Informational);
				case ePowerState.Warming:
					return new Activity(Activity.ePriority.Low, "Powered", "Warming Up", eSeverity.Informational);
				case ePowerState.Cooling:
					return new Activity(Activity.ePriority.Low, "Powered", "Cooling Down", eSeverity.Informational);
				default:
					throw new ArgumentOutOfRangeException("powerState");
			}
		}
	}
}
