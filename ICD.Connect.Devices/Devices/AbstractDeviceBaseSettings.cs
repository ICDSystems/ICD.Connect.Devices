using ICD.Common.Utils.Xml;
using ICD.Connect.Devices.Telemetry.DeviceInfo;
using ICD.Connect.Devices.Telemetry.DeviceInfo.Configured.Settings;
using ICD.Connect.Settings;

namespace ICD.Connect.Devices
{
	public abstract class AbstractDeviceBaseSettings : AbstractSettings, IDeviceBaseSettings
	{
		private readonly ConfiguredDeviceInfoSettings m_ConfiguredDeviceInfo;

		public ConfiguredDeviceInfoSettings ConfiguredDeviceInfo { get { return m_ConfiguredDeviceInfo; } }

		protected AbstractDeviceBaseSettings()
		{
			m_ConfiguredDeviceInfo = new ConfiguredDeviceInfoSettings();
		}

		/// <summary>
		/// Writes property elements to xml.
		/// </summary>
		/// <param name="writer"></param>
		protected override void WriteElements(IcdXmlTextWriter writer)
		{
			base.WriteElements(writer);

			ConfiguredDeviceInfo.WriteElements(writer);
	
		}

		/// <summary>
		/// Updates the settings from xml.
		/// </summary>
		/// <param name="xml"></param>
		public override void ParseXml(string xml)
		{
			base.ParseXml(xml);

			ConfiguredDeviceInfo.ParseXml(xml);
		}
	}
}
