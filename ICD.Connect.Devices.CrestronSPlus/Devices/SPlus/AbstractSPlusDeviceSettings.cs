﻿using System;
using ICD.Common.Utils.Xml;
using ICD.Connect.Devices.Telemetry.DeviceInfo.Configured.Settings;

namespace ICD.Connect.Devices.CrestronSPlus.Devices.SPlus
{
	public abstract class AbstractSPlusDeviceSettings : AbstractSPlusDeviceBaseSettings, ISPlusDeviceSettings
	{
		private const string ROOM_CRITTICAL_ELEMENT = "RoomCritical";
		private readonly ConfiguredDeviceInfoSettings m_ConfiguredDeviceInfo;

		#region Properties

		public ConfiguredDeviceInfoSettings ConfiguredDeviceInfo { get { return m_ConfiguredDeviceInfo; } }

		/// <summary>
		/// Specifies that the room is critical to room operation.
		/// </summary>
		public bool RoomCritical { get; set; }

		/// <summary>
		/// Gets/sets the manufacturer for this device.
		/// </summary>
		public string Manufacturer
		{
			get { return ConfiguredDeviceInfo.Make; }
			set { ConfiguredDeviceInfo.Make = value; }
		}

		/// <summary>
		/// Gets/sets the model number for this device.
		/// </summary>
		public string Model
		{
			get { return ConfiguredDeviceInfo.Model; }
			set { ConfiguredDeviceInfo.Model = value; }
		}

		/// <summary>
		/// Gets/sets the serial number for this device.
		/// </summary>
		public string SerialNumber
		{
			get { return ConfiguredDeviceInfo.SerialNumber; }
			set { ConfiguredDeviceInfo.SerialNumber = value; }
		}

		/// <summary>
		/// Gets/sets the purchase date for this device.
		/// </summary>
		public DateTime PurchaseDate
		{
			get { return ConfiguredDeviceInfo.PurchaseDate ?? default(DateTime); }
			set { ConfiguredDeviceInfo.PurchaseDate = value; }
		}

		#endregion

		protected AbstractSPlusDeviceSettings()
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
			writer.WriteElementString(ROOM_CRITTICAL_ELEMENT, IcdXmlConvert.ToString(RoomCritical));
		}

		/// <summary>
		/// Updates the settings from xml.
		/// </summary>
		/// <param name="xml"></param>
		public override void ParseXml(string xml)
		{
			base.ParseXml(xml);

			ConfiguredDeviceInfo.ParseXml(xml);
			RoomCritical = XmlUtils.TryReadChildElementContentAsBoolean(xml, ROOM_CRITTICAL_ELEMENT) ?? false;
		}

		#endregion
	}
}
