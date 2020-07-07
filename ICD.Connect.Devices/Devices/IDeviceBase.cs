using System;
using System.Collections.Generic;
using ICD.Common.Utils;
using ICD.Connect.API.Attributes;
using ICD.Connect.Devices.EventArguments;
using ICD.Connect.Devices.Proxies.Devices;
using ICD.Connect.Devices.Telemetry.DeviceInfo;
using ICD.Connect.Settings.Originators;
using ICD.Connect.Telemetry.Attributes;

namespace ICD.Connect.Devices
{
	public interface IDeviceBase : IOriginator, IStateDisposable
	{
		/// <summary>
		/// Device Info Telemetry, configured from DAV
		/// </summary>
		IConfiguredDeviceInfoTelemetry ConfiguredDeviceInfo { get; }

		/// <summary>
		/// Device Info Telemetry, monitored from the device itself
		/// </summary>
		IMonitoredDeviceInfoTelemetry MonitoredDeviceInfo { get; }

		/// <summary>
		/// Device Info Telemetry, returns both monitored and configured telemetry
		/// </summary>
		[CollectionTelemetry("DeviceInfo")]
		IEnumerable<IDeviceInfoTelemetry> DeviceInfo { get; }

		/// <summary>
		/// Raised when the device goes online/offline.
		/// </summary>
		[ApiEvent(DeviceBaseApi.EVENT_IS_ONLINE, DeviceBaseApi.HELP_EVENT_IS_ONLINE)]
		[EventTelemetry(DeviceTelemetryNames.ONLINE_STATE_CHANGED)]
		event EventHandler<DeviceBaseOnlineStateApiEventArgs> OnIsOnlineStateChanged;

		/// <summary>
		/// Returns true if the device hardware is detected by the system.
		/// </summary>
		[ApiProperty(DeviceBaseApi.PROPERTY_IS_ONLINE, DeviceBaseApi.HELP_PROPERTY_IS_ONLINE)]
		[PropertyTelemetry(DeviceTelemetryNames.ONLINE_STATE, null, DeviceTelemetryNames.ONLINE_STATE_CHANGED)]
		bool IsOnline { get; }
	}
}
