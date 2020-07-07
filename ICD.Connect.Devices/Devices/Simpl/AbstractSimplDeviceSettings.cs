using System;
using ICD.Common.Utils.Xml;
using ICD.Connect.Devices.Telemetry.DeviceInfo.Configured.Settings;

namespace ICD.Connect.Devices.Simpl
{
	public abstract class AbstractSimplDeviceSettings : AbstractSimplDeviceBaseSettings, ISimplDeviceSettings
	{
		private const string ELEMENT_MANUFACTURER = "Manufacturer";
		private const string ELEMENT_MODEL = "Model";
		private const string ELEMENT_SERIAL_NUMBER = "SerialNumber";
		private const string ELEMENT_PURCHASE_DATE = "PurchaseDate";


		private readonly ConfiguredDeviceInfoSettings m_ConfiguredDeviceInfo;

		#region Properties

		public ConfiguredDeviceInfoSettings ConfiguredDeviceInfo { get { return m_ConfiguredDeviceInfo; } }

		/// <summary>
		/// Gets/sets the manufacturer for this device.
		/// </summary>
		public string Manufacturer { get; set; }

		/// <summary>
		/// Gets/sets the model number for this device.
		/// </summary>
		public string Model { get; set; }

		/// <summary>
		/// Gets/sets the serial number for this device.
		/// </summary>
		public string SerialNumber { get; set; }

		/// <summary>
		/// Gets/sets the purchase date for this device.
		/// </summary>
		public DateTime PurchaseDate { get; set; }

		#endregion

		protected AbstractSimplDeviceSettings()
		{
			m_ConfiguredDeviceInfo = new ConfiguredDeviceInfoSettings();
		}

		#region Methods

		/// <summary>
		/// Writes property elements to xml.
		/// </summary>
		/// <param name="writer"></param>
		protected override void WriteElements(IcdXmlTextWriter writer)
		{
			base.WriteElements(writer);

			ConfiguredDeviceInfo.WriteElements(writer);

			writer.WriteElementString(ELEMENT_MANUFACTURER, Manufacturer);
			writer.WriteElementString(ELEMENT_MODEL, Model);
			writer.WriteElementString(ELEMENT_SERIAL_NUMBER, SerialNumber);
			writer.WriteElementString(ELEMENT_PURCHASE_DATE, IcdXmlConvert.ToString(PurchaseDate));
		}

		/// <summary>
		/// Updates the settings from xml.
		/// </summary>
		/// <param name="xml"></param>
		public override void ParseXml(string xml)
		{
			base.ParseXml(xml);

			Manufacturer = XmlUtils.TryReadChildElementContentAsString(xml, ELEMENT_MANUFACTURER);
			Model = XmlUtils.TryReadChildElementContentAsString(xml, ELEMENT_MODEL);
			SerialNumber = XmlUtils.TryReadChildElementContentAsString(xml, ELEMENT_SERIAL_NUMBER);
			PurchaseDate = XmlUtils.TryReadChildElementContentAsDateTime(xml, ELEMENT_PURCHASE_DATE) ?? DateTime.MinValue;

			ConfiguredDeviceInfo.ParseXml(xml);
		}

		#endregion
	}
}
