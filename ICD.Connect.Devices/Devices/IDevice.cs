using System;
using ICD.Connect.API.Attributes;
using ICD.Connect.Devices.Controls;
using ICD.Connect.Devices.EventArguments;
using ICD.Connect.Devices.Proxies.Devices;
using ICD.Connect.Telemetry.Attributes;

namespace ICD.Connect.Devices
{
	/// <summary>
	/// Interface for all hardware devices.
	/// </summary>
	[ApiClass(typeof(ProxyDevice), typeof(IDeviceBase))]
	public interface IDevice : IDeviceBase
	{
		/// <summary>
		/// Raised when control availability for the device changes.
		/// </summary>
		[ApiEvent(DeviceBaseApi.EVENT_CONTROLS_AVAILABLE, DeviceBaseApi.HELP_EVENT_CONTROLS_AVAILABLE)]
		event EventHandler<DeviceBaseControlsAvailableApiEventArgs> OnControlsAvailableChanged;

		/// <summary>
		/// Gets the controls for this device.
		/// </summary>
		[ApiNodeGroup(DeviceBaseApi.NODE_GROUP_CONTROLS, DeviceBaseApi.HELP_NODE_GROUP_CONTROLS)]
		[CollectionTelemetry("Controls")]
		DeviceControlsCollection Controls { get; }

		/// <summary>
		/// Gets if controls are available.
		/// </summary>
		[ApiProperty(DeviceBaseApi.PROPERTY_CONTROLS_AVAILABLE, DeviceBaseApi.HELP_PROPERTY_CONTROLS_AVAILABLE)]
		bool ControlsAvailable { get; }

		/// <summary>
		/// Gets/sets the manufacturer for this device.
		/// </summary>
		[StaticPropertyTelemetry(DeviceTelemetryNames.MANUFACTURER)]
		string Manufacturer { get; set; }

		/// <summary>
		/// Gets/sets the model number for this device.
		/// </summary>
		[StaticPropertyTelemetry(DeviceTelemetryNames.MODEL)]
		string Model { get; set; }

		/// <summary>
		/// Gets/sets the serial number for this device.
		/// </summary>
		[StaticPropertyTelemetry(DeviceTelemetryNames.SERIAL_NUMBER)]
		string SerialNumber { get; set; }

		/// <summary>
		/// Gets/sets the purchase date for this device.
		/// </summary>
		[StaticPropertyTelemetry(DeviceTelemetryNames.PURCHASE_DATE)]
		DateTime PurchaseDate { get; set; }
	}
}
