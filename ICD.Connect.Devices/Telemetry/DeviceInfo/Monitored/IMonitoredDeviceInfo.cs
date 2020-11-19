using System;
using ICD.Common.Properties;
using ICD.Common.Utils.EventArguments;
using ICD.Connect.Telemetry.Attributes;

namespace ICD.Connect.Devices.Telemetry.DeviceInfo.Monitored
{
	public interface IMonitoredDeviceInfo : IDeviceInfo
	{
		[EventTelemetry(DeviceTelemetryNames.DEVICE_FIRMWARE_VERSION_CHANGED)]
		event EventHandler<StringEventArgs> OnFirmwareVersionChanged;

		[EventTelemetry(DeviceTelemetryNames.DEVICE_FIRMWARE_DATE_CHANGED)]
		event EventHandler<DateTimeNullableEventArgs> OnFirmwareDateChanged;

		[EventTelemetry(DeviceTelemetryNames.DEVICE_UPTIME_CHANGED)]
		event EventHandler<DateTimeNullableEventArgs> OnUptimeStartChanged;

		[CanBeNull]
		[PropertyTelemetry(DeviceTelemetryNames.DEVICE_FIRMWARE_VERSION, null, DeviceTelemetryNames.DEVICE_FIRMWARE_VERSION_CHANGED)]
		string FirmwareVersion { get; set; }
		
		[CanBeNull]
		[PropertyTelemetry(DeviceTelemetryNames.DEVICE_FIRMWARE_DATE, null, DeviceTelemetryNames.DEVICE_FIRMWARE_DATE_CHANGED)]
		DateTime? FirmwareDate { get; set; }

		[CanBeNull]
		[PropertyTelemetry(DeviceTelemetryNames.DEVICE_UPTIME, null, DeviceTelemetryNames.DEVICE_UPTIME_CHANGED)]
		DateTime? UptimeStart { get; set; }
	}
}