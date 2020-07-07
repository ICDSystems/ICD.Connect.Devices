using ICD.Common.Utils.Xml;

namespace ICD.Connect.Devices.Telemetry.DeviceInfo.Configured.Settings
{
	public interface IConfiguredDeviceInfoSettingsBase
	{
		/// <summary>
		/// Write the settings out to xml
		/// </summary>
		/// <param name="writer"></param>
		void WriteElements(IcdXmlTextWriter writer);

		/// <summary>
		/// Parse the given XML into the device info
		/// </summary>
		/// <param name="xml"></param>
		void ParseXml(string xml);

		/// <summary>
		/// Clear the settings
		/// </summary>
		void Clear();
	}
}