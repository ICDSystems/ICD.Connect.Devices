﻿using ICD.Connect.Settings;

namespace ICD.Connect.Devices
{
	public abstract class AbstractDeviceSettings : AbstractDeviceBaseSettings, IDeviceSettings
	{
		public const string DEVICE_ELEMENT = "Device";

		/// <summary>
		/// Gets the xml element.
		/// </summary>
		protected override string Element { get { return DEVICE_ELEMENT; } }

		/// <summary>
		/// Parses the xml and applies the properties to the instance.
		/// </summary>
		/// <param name="instance"></param>
		/// <param name="xml"></param>
		protected static void ParseXml(AbstractDeviceSettings instance, string xml)
		{
			AbstractDeviceBaseSettings.ParseXml(instance, xml);
		}
	}
}
