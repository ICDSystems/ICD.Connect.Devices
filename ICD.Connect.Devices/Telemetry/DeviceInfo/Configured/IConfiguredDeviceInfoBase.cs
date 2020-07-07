using ICD.Connect.Devices.Telemetry.DeviceInfo.Configured.Settings;

namespace ICD.Connect.Devices.Telemetry.DeviceInfo.Configured
{
	public interface IConfiguredDeviceInfoBase<TSettings>
		where TSettings : IConfiguredDeviceInfoSettingsBase
	{
		/// <summary>
		/// Apply the configuration from the settings
		/// </summary>
		/// <param name="settings"></param>
		void ApplySettings(TSettings settings);

		/// <summary>
		/// Copy the configuration to the given settings
		/// </summary>
		/// <param name="settings"></param>
		void CopySettings(TSettings settings);

		/// <summary>
		/// Clear the items from settings
		/// </summary>
		void ClearSettings();
	}
}