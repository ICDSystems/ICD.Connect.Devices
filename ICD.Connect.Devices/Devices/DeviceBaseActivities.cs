using ICD.Common.Logging.Activities;
using ICD.Common.Utils.Services.Logging;

namespace ICD.Connect.Devices
{
	public static class DeviceBaseActivities
	{
		public static Activity GetIsOnlineActivity(bool isOnline)
		{
			return isOnline
				       ? new Activity(Activity.ePriority.Low, "Is Online", "Online", eSeverity.Informational)
					   : new Activity(Activity.ePriority.High, "Is Online", "Offline", eSeverity.Error);
		}
	}
}
