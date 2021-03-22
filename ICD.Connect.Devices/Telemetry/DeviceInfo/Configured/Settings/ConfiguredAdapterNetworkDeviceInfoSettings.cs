using ICD.Common.Utils.Xml;

namespace ICD.Connect.Devices.Telemetry.DeviceInfo.Configured.Settings
{
	public sealed class ConfiguredAdapterNetworkDeviceInfoSettings : IConfiguredDeviceInfoSettingsBase
	{
		private const string ELEMENT = "Adapter";
		private const string ATTRIBUTE_ADDRESS = "address";
		private const string ELEMENT_NAME = "Name";
		private const string ELEMENT_MAC_ADDRESS = "MacAddress";
		private const string ELEMENT_DHCP = "DHCP";
		private const string ELEMENT_IPV4_ADDRESS = "Ipv4Address";
		private const string ELEMENT_IPV4_SUBNET = "Ipv4Subnet";
		private const string ELEMENT_IPV4_GATEWAY = "Ipv4Gateway";

		public int Address { get; set; }

		public string Name { get; set; }

		public IcdPhysicalAddress MacAddress { get; set; }

		public bool? Dhcp { get; set; }

		public string Ipv4Address { get; set; }

		public string Ipv4SubnetMask { get; set; }

		public string Ipv4Gateway { get; set; }

		/// <summary>
		/// Write the settings out to xml
		/// </summary>
		/// <param name="writer"></param>
		public void WriteElements(IcdXmlTextWriter writer)
		{
			writer.WriteStartElement(ELEMENT);
			writer.WriteAttributeString(ATTRIBUTE_ADDRESS, IcdXmlConvert.ToString(Address));
			{
				writer.WriteElementString(ELEMENT_NAME, Name);
				writer.WriteElementString(ELEMENT_MAC_ADDRESS, IcdXmlConvert.ToString(MacAddress));
				writer.WriteElementString(ELEMENT_DHCP, IcdXmlConvert.ToString(Dhcp));
				writer.WriteElementString(ELEMENT_IPV4_ADDRESS, Ipv4Address);
				writer.WriteElementString(ELEMENT_IPV4_SUBNET, Ipv4SubnetMask);
				writer.WriteElementString(ELEMENT_IPV4_GATEWAY, Ipv4Gateway);
			}
			writer.WriteEndElement();
		}

		/// <summary>
		/// Parse the given XML into the device info
		/// </summary>
		/// <param name="xml"></param>
		public void ParseXml(string xml)
		{
			string mac = XmlUtils.TryReadChildElementContentAsString(xml, ELEMENT_MAC_ADDRESS) ?? string.Empty;
			IcdPhysicalAddress macAddress;
			IcdPhysicalAddress.TryParse(mac, out macAddress);

			Address = XmlUtils.GetAttributeAsInt(xml, ATTRIBUTE_ADDRESS);
			Name = XmlUtils.TryReadChildElementContentAsString(xml, ELEMENT_NAME);
			MacAddress = macAddress;
			Dhcp = XmlUtils.TryReadChildElementContentAsBoolean(xml, ELEMENT_DHCP);
			Ipv4Address = XmlUtils.TryReadChildElementContentAsString(xml, ELEMENT_IPV4_ADDRESS);
			Ipv4SubnetMask = XmlUtils.TryReadChildElementContentAsString(xml, ELEMENT_IPV4_SUBNET);
			Ipv4Gateway = XmlUtils.TryReadChildElementContentAsString(xml, ELEMENT_IPV4_GATEWAY);
		}

		/// <summary>
		/// Clear the settings
		/// </summary>
		public void Clear()
		{
			Address = 0;
			Name = null;
			MacAddress = null;
			Dhcp = null;
			Ipv4Address = null;
			Ipv4SubnetMask = null;
			Ipv4Gateway = null;
		}
	}
}