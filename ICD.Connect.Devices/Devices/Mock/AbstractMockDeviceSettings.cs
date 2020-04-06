using ICD.Common.Utils.Xml;

namespace ICD.Connect.Devices.Mock
{
	public class AbstractMockDeviceSettings : AbstractDeviceSettings, IMockDeviceSettings
	{
		public bool DefaultOffline { get; set; }

		/// <summary>
		/// Writes property elements to xml.
		/// </summary>
		/// <param name="writer"></param>
		protected override void WriteElements(IcdXmlTextWriter writer)
		{
			base.WriteElements(writer);

			MockDeviceSettingsHelper.WriteElements(this, writer);
		}

		/// <summary>
		/// Updates the settings from xml.
		/// </summary>
		/// <param name="xml"></param>
		public override void ParseXml(string xml)
		{
			base.ParseXml(xml);

			MockDeviceSettingsHelper.ParseXml(this, xml);
		}
	}
}