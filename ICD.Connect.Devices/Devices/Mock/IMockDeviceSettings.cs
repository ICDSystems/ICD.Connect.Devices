using System;
using ICD.Common.Properties;
using ICD.Common.Utils.Xml;

namespace ICD.Connect.Devices.Mock
{
	public interface IMockDeviceSettings : IDeviceSettings
	{
		 bool DefaultOffline { get; set; }
	}

	public static class MockDeviceSettingsHelper
	{
		private const string DEFAULT_OFFLINE_ELEMENT = "DefaultOffline";

		public static void WriteElements([NotNull] IMockDeviceSettings settings, [NotNull] IcdXmlTextWriter writer)
		{
			if (settings == null)
				throw new ArgumentNullException("settings");

			if (writer == null)
				throw new ArgumentNullException("writer");

			writer.WriteElementString(DEFAULT_OFFLINE_ELEMENT, IcdXmlConvert.ToString(settings.DefaultOffline));
		}

		public static void ParseXml([NotNull] IMockDeviceSettings settings, [NotNull] string xml)
		{
			if (settings == null)
				throw new ArgumentNullException("settings");
			
			if (xml == null)
				throw new ArgumentNullException("xml");

			settings.DefaultOffline = XmlUtils.TryReadChildElementContentAsBoolean(xml, DEFAULT_OFFLINE_ELEMENT) ?? false;
		}
	}
}