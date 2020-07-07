using System;
using ICD.Common.Properties;
using ICD.Common.Utils.EventArguments;
using ICD.Connect.Telemetry.Attributes;

namespace ICD.Connect.Devices.Telemetry.DeviceInfo
{
	public interface IConfiguredDeviceInfoTelemetry : IDeviceInfoTelemetry, IConfiguredDeviceInfoTelemetryBase<ConfiguredDeviceInfoSettings>
	{
		[EventTelemetry(DeviceTelemetryNames.DEVICE_PURCHASE_DATE_CHANGED)]
		event EventHandler<DateTimeNullableEventArgs> OnPurchaseDateChanged;

		[PropertyTelemetry(DeviceTelemetryNames.DEVICE_PURCHASE_DATE, null, DeviceTelemetryNames.DEVICE_PURCHASE_DATE_CHANGED)]
		[CanBeNull]
		DateTime? PurchaseDate { get; set; }
		
	}
}