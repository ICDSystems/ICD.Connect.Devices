using ICD.Connect.Settings;
using ICD.Connect.Settings.Attributes.SettingsProperties;

namespace ICD.Connect.Devices.Points
{
	public interface IPointSettings : ISettings
	{
		/// <summary>
		/// Device id
		/// </summary>
		[OriginatorIdSettingsProperty(typeof(IDeviceBase))]
		int DeviceId { get; set; }

		/// <summary>
		/// Control id.
		/// </summary>
		int ControlId { get; set; }
	}
}