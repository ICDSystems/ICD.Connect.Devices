using System;
using System.Collections.Generic;
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
	}
}
