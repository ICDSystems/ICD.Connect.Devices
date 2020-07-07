using System.Collections.Generic;
using ICD.Common.Utils.Xml;
using ICD.Connect.Devices.Telemetry.DeviceInfo.NetworkInfo.AdapterInfo;

namespace ICD.Connect.Devices.Telemetry.DeviceInfo.NetworkInfo
{
	public sealed class ConfiguredNetworkDeviceInfoTelemetrySettings : IConfiguredDeviceInfoTelemetrySettingsBase
	{
		private const string ELEMENT_NETWORK_INFO = "NetworkInfo";
		private const string ELEMENT_HOSTNAME = "Hostname";
		private const string ELEMENT_DNS = "Dns";
		private const string ELEMENT_ADAPTERS = "Adapters";

		private readonly List<ConfiguredAdapterNetworkDeviceInfoTelemetrySettings> m_Adapters;

		public string Hostname { get; set; }

		public string Dns { get; set; }

		public List<ConfiguredAdapterNetworkDeviceInfoTelemetrySettings> Adapters { get { return m_Adapters; } }

		public ConfiguredNetworkDeviceInfoTelemetrySettings()
		{
			m_Adapters = new List<ConfiguredAdapterNetworkDeviceInfoTelemetrySettings>();
		}

		/// <summary>
		/// Write the settings out to xml
		/// </summary>
		/// <param name="writer"></param>
		public void WriteElements(IcdXmlTextWriter writer)
		{
			writer.WriteStartElement(ELEMENT_NETWORK_INFO);
			{
				writer.WriteEndElement();
				writer.WriteElementString(ELEMENT_HOSTNAME, Hostname);
				writer.WriteElementString(ELEMENT_DNS, Dns);

				writer.WriteStartElement(ELEMENT_ADAPTERS);
				{
					foreach (var adapter in Adapters)
						adapter.WriteElements(writer);
				}
				writer.WriteEndElement();
			}
			writer.WriteEndElement();
		}

		/// <summary>
		/// Parse the given XML into the device info
		/// </summary>
		/// <param name="xml"></param>
		public void ParseXml(string xml)
		{
			string innerXml;

			XmlUtils.TryGetChildElementAsString(xml, ELEMENT_NETWORK_INFO, out innerXml);

			if (string.IsNullOrEmpty(innerXml))
				return;

			Hostname = XmlUtils.TryReadChildElementContentAsString(innerXml, ELEMENT_HOSTNAME);
			Dns = XmlUtils.TryReadChildElementContentAsString(innerXml, ELEMENT_DNS);
			Adapters.Clear();

			string adaptersElement;
			XmlUtils.TryGetChildElementAsString(innerXml, ELEMENT_ADAPTERS, out adaptersElement);
			if (!string.IsNullOrEmpty(adaptersElement))
			{

				IEnumerable<string> adaptersXml = XmlUtils.GetChildElementsAsString(adaptersElement);
				foreach (string adapterXml in adaptersXml)
				{
					var adapter = new ConfiguredAdapterNetworkDeviceInfoTelemetrySettings();
					adapter.ParseXml(adapterXml);
					Adapters.Add(adapter);
				}
			}
		}

		/// <summary>
		/// Clear the settings
		/// </summary>
		public void Clear()
		{
			Hostname = null;
			Dns = null;
			//todo: Dispose Adapters
			Adapters.Clear();
		}
	}
}