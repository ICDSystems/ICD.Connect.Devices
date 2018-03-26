﻿namespace ICD.Connect.Devices
{
	public abstract class AbstractDeviceSettings : AbstractDeviceBaseSettings, IDeviceSettings
	{
		private const string DEVICE_ELEMENT = "Device";

		/// <summary>
		/// Gets the xml element.
		/// </summary>
		protected override string Element { get { return DEVICE_ELEMENT; } }
	}
}