using System;
using ICD.Common.Utils.EventArguments;
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
		/// Raised when the model changes.
		/// </summary>
		[EventTelemetry(DeviceTelemetryNames.DEVICE_MODEL_CHANGED)]
		event EventHandler<StringEventArgs> OnModelChanged;

		/// <summary>
		/// Raised when the serial number changes.
		/// </summary>
		[EventTelemetry(DeviceTelemetryNames.DEVICE_SERIAL_NUMBER_CHANGED)]
		event EventHandler<StringEventArgs> OnSerialNumberChanged;

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
		[PropertyTelemetry(DeviceTelemetryNames.CONFIGURED_MANUFACTURER, null, null)]
		string ConfiguredManufacturer { get; set; }

		/// <summary>
		/// Gets/sets the model number for this device.
		/// </summary>
		[PropertyTelemetry(DeviceTelemetryNames.CONFIGURED_MODEL, null, null)]
		string ConfiguredModel { get; set; }

		/// <summary>
		/// Gets/sets the serial number for this device.
		/// </summary>
		[PropertyTelemetry(DeviceTelemetryNames.CONFIGURED_SERIAL_NUMBER, null, null)]
		string ConfiguredSerialNumber { get; set; }

		/// <summary>
		/// Gets/sets the purchase date for this device.
		/// </summary>
		[PropertyTelemetry(DeviceTelemetryNames.CONFIGURED_PURCHASE_DATE, null, null)]
		DateTime ConfiguredPurchaseDate { get; set; }

		/// <summary>
		/// Gets the discovered model.
		/// </summary>
		[PropertyTelemetry(DeviceTelemetryNames.DEVICE_MODEL, null, DeviceTelemetryNames.DEVICE_MODEL_CHANGED)]
		string Model { get; }

		/// <summary>
		/// Gets the discovered serial number.
		/// </summary>
		[PropertyTelemetry(DeviceTelemetryNames.DEVICE_SERIAL_NUMBER, null, DeviceTelemetryNames.DEVICE_SERIAL_NUMBER_CHANGED)]
		string SerialNumber { get; }
	}
}
