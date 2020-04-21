using System;

namespace ICD.Connect.Devices.Proxies.Devices
{
	public abstract class AbstractProxyDeviceSettings : AbstractProxyDeviceBaseSettings, IProxyDeviceSettings
	{
		/// <summary>
		/// Gets/sets the manufacturer for this device.
		/// </summary>
		string IDeviceSettings.Manufacturer { get; set; }

		/// <summary>
		/// Gets/sets the model number for this device.
		/// </summary>
		string IDeviceSettings.Model { get; set; }

		/// <summary>
		/// Gets/sets the serial number for this device.
		/// </summary>
		string IDeviceSettings.SerialNumber { get; set; }

		/// <summary>
		/// Gets/sets the purchase date for this device.
		/// </summary>
		DateTime IDeviceSettings.PurchaseDate { get; set; }
	}
}
