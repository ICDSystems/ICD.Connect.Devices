using System;
using ICD.Common.Properties;
using ICD.Common.Utils.Xml;
using ICD.Connect.Devices.Telemetry.DeviceInfo.NetworkInfo;

namespace ICD.Connect.Devices.Telemetry.DeviceInfo
{
	public sealed class ConfiguredDeviceInfoSettings : IConfiguredDeviceInfoTelemetrySettingsBase
	{
		private const string ELEMENT_CONFIGURED_DEVICE_INFO = "ConfiguredDeviceInfo";
		private const string ELEMENT_MAKE = "Make";
		private const string ELEMENT_MODEL = "Model";
		private const string ELEMENT_SERIAL_NUMBER = "SerialNumber";
		private const string ELEMENT_PURCHASE_DATE = "PurchaseDate";
		

		private readonly ConfiguredNetworkDeviceInfoTelemetrySettings m_NetworkInfo;

		[CanBeNull]
		public string Make { get; set; }
		[CanBeNull]
		public string Model { get; set; }
		[CanBeNull]
		public string SerialNumber { get; set; }
		[CanBeNull]
		public DateTime? PurchaseDate { get; set; }

		[NotNull]
		public ConfiguredNetworkDeviceInfoTelemetrySettings NetworkInfo { get { return m_NetworkInfo; } }

		public ConfiguredDeviceInfoSettings()
		{
			m_NetworkInfo = new ConfiguredNetworkDeviceInfoTelemetrySettings();
		}

		/// <summary>
		/// Write the settings out to xml
		/// </summary>
		/// <param name="writer"></param>
		public void WriteElements(IcdXmlTextWriter writer)
		{
			if (writer == null)
				throw new ArgumentNullException("writer");

			writer.WriteStartElement(ELEMENT_CONFIGURED_DEVICE_INFO);
			{
				writer.WriteElementString(ELEMENT_MAKE, Make);
				writer.WriteElementString(ELEMENT_MODEL, Model);
				writer.WriteElementString(ELEMENT_SERIAL_NUMBER, SerialNumber);
				//Todo: Confirm this works with DateTime Nullable!
				writer.WriteElementString(ELEMENT_PURCHASE_DATE, IcdXmlConvert.ToString(PurchaseDate));

				NetworkInfo.WriteElements(writer);
			}

			
		}

		/// <summary>
		/// Parse the given XML into the device info
		/// </summary>
		/// <param name="xml"></param>
		public void ParseXml(string xml)
		{
			Clear();

			if (String.IsNullOrEmpty(xml))
				return;

			
			string innerXml;
			XmlUtils.TryGetChildElementAsString(xml, ELEMENT_CONFIGURED_DEVICE_INFO, out innerXml);
			if (!String.IsNullOrEmpty(innerXml))
				ParseInnerXml(innerXml);
		}

		private void ParseInnerXml(string xml)
		{
			Make = XmlUtils.TryReadChildElementContentAsString(xml, ELEMENT_MAKE);
			Model = XmlUtils.TryReadChildElementContentAsString(xml, ELEMENT_MODEL);
			SerialNumber = XmlUtils.TryReadChildElementContentAsString(xml, ELEMENT_SERIAL_NUMBER);
			PurchaseDate = XmlUtils.TryReadChildElementContentAsDateTime(xml, ELEMENT_PURCHASE_DATE);

			NetworkInfo.ParseXml(xml);
		}

		/// <summary>
		/// Clear the settings
		/// </summary>
		public void Clear()
		{
			Make = null;
			Model = null;
			SerialNumber = null;
			PurchaseDate = null;
			NetworkInfo.Clear();
		}
	}
}