using System;
using ICD.Common.Properties;
using ICD.Common.Utils.EventArguments;
using ICD.Connect.Telemetry.Attributes;

namespace ICD.Connect.Devices.Telemetry.DeviceInfo
{
	public interface IMonitoredDeviceInfoTelemetry : IDeviceInfoTelemetry
	{
		[EventTelemetry(DeviceTelemetryNames.DEVICE_FIRMWARE_VERSION_CHANGED)]
		event EventHandler<StringEventArgs> OnFirmwareVersionChanged;

		[EventTelemetry(DeviceTelemetryNames.DEVICE_FIRMWARE_DATE_CHANGED)]
		event EventHandler<DateTimeNullableEventArgs> OnFirmwareDateChanged;

		[EventTelemetry(DeviceTelemetryNames.DEVICE_UPTIME_CHANGED)]
		event EventHandler<DateTimeNullableEventArgs> OnUptimeStartChanged;

		[EventTelemetry(DeviceTelemetryNames.DEVICE_REBOOT_SUPPORTED_CHANGED)]
		event EventHandler<BoolEventArgs> OnRebootSupportedChanged;

		[CanBeNull]
		[PropertyTelemetry(DeviceTelemetryNames.DEVICE_FIRMWARE_VERSION, null, DeviceTelemetryNames.DEVICE_FIRMWARE_VERSION_CHANGED)]
		string FirmwareVersion { get; set; }
		
		[CanBeNull]
		[PropertyTelemetry(DeviceTelemetryNames.DEVICE_FIRMWARE_DATE, null, DeviceTelemetryNames.DEVICE_FIRMWARE_DATE_CHANGED)]
		DateTime? FirmwareDate { get; set; }
		
		[PropertyTelemetry(DeviceTelemetryNames.DEVICE_UPTIME, null, DeviceTelemetryNames.DEVICE_UPTIME_CHANGED)]
		[CanBeNull]
		DateTime? UptimeStart { get; set; }


		[PropertyTelemetry(DeviceTelemetryNames.DEVICE_REBOOT_SUPPORTED, null, DeviceTelemetryNames.DEVICE_REBOOT_SUPPORTED_CHANGED)]
		bool RebootSupported { get; set; }

		[MethodTelemetry(DeviceTelemetryNames.DEVICE_REBOOT)]
		void Reboot();

	}
}