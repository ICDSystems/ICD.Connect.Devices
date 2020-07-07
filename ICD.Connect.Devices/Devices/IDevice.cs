using System;
using System.Collections.Generic;
using ICD.Common.Utils.EventArguments;
using ICD.Connect.API.Attributes;
using ICD.Connect.Devices.Controls;
using ICD.Connect.Devices.EventArguments;
using ICD.Connect.Devices.Proxies.Devices;
using ICD.Connect.Devices.Telemetry.DeviceInfo;
using ICD.Connect.Devices.Telemetry.DeviceInfo.Configured;
using ICD.Connect.Devices.Telemetry.DeviceInfo.Monitored;
using ICD.Connect.Telemetry.Attributes;

namespace ICD.Connect.Devices
{

	/// <summary>
	/// Interface for all hardware devices.
	/// </summary>
	[ApiClass(typeof(ProxyDevice), typeof(IDeviceBase))]
	[ExternalTelemetry("Device Telemetry", typeof(DeviceExternalTelemetryProvider))]
	public interface IDevice : IDeviceBase
	{

		/// <summary>
		/// Device Info Telemetry, configured from DAV
		/// </summary>
		IConfiguredDeviceInfo ConfiguredDeviceInfo { get; }

		/// <summary>
		/// Device Info Telemetry, monitored from the device itself
		/// </summary>
		IMonitoredDeviceInfo MonitoredDeviceInfo { get; }

		/// <summary>
		/// Device Info Telemetry, returns both monitored and configured telemetry
		/// </summary>
		[CollectionTelemetry("DeviceInfo")]
		IEnumerable<IDeviceInfo> DeviceInfo { get; }

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
