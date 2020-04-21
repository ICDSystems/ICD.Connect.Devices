﻿using System;
using ICD.Connect.Settings;

namespace ICD.Connect.Devices
{
	public interface IDeviceBaseSettings : ISettings
	{
		/// <summary>
		/// Gets/sets the manufacturer for this device.
		/// </summary>
		string Manufacturer { get; set; }

		/// <summary>
		/// Gets/sets the model number for this device.
		/// </summary>
		string Model { get; set; }

		/// <summary>
		/// Gets/sets the serial number for this device.
		/// </summary>
		string SerialNumber { get; set; }

		/// <summary>
		/// Gets/sets the purchase date for this device.
		/// </summary>
		DateTime PurchaseDate { get; set; }
	}
}
