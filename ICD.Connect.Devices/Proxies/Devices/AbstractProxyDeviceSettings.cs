using System;
using ICD.Common.Utils.Xml;
using ICD.Connect.Devices.Telemetry.DeviceInfo.Configured.Settings;

namespace ICD.Connect.Devices.Proxies.Devices
{
	public abstract class AbstractProxyDeviceSettings : AbstractProxyDeviceBaseSettings, IProxyDeviceSettings
	{
		private readonly ConfiguredDeviceInfoSettings m_ConfiguredDeviceInfo;

		#region Properties

		/// <summary>
		/// Gets/sets the manufacturer for this device.
		/// </summary>
		string IDeviceSettings.Manufacturer
		{
			get { return ConfiguredDeviceInfo.Make; }
			set { ConfiguredDeviceInfo.Make = value; }
		}

		/// <summary>
		/// Gets/sets the model number for this device.
		/// </summary>
		string IDeviceSettings.Model
		{
			get { return ConfiguredDeviceInfo.Model; }
			set { ConfiguredDeviceInfo.Model = value; }
		}

		/// <summary>
		/// Gets/sets the serial number for this device.
		/// </summary>
		string IDeviceSettings.SerialNumber
		{
			get { return ConfiguredDeviceInfo.SerialNumber; }
			set { ConfiguredDeviceInfo.SerialNumber = value; }
		}

		/// <summary>
		/// Gets/sets the purchase date for this device.
		/// </summary>
		DateTime IDeviceSettings.PurchaseDate
		{
			get { return ConfiguredDeviceInfo.PurchaseDate ?? default(DateTime); }
			set { ConfiguredDeviceInfo.PurchaseDate = value; }
		}

		public ConfiguredDeviceInfoSettings ConfiguredDeviceInfo { get { return m_ConfiguredDeviceInfo; } }

		#endregion

		protected AbstractProxyDeviceSettings()
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

		#endregion
	}
}
