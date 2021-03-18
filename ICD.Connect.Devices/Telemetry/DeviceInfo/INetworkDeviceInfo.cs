using System;
using ICD.Common.Utils.EventArguments;
using ICD.Connect.Telemetry.Attributes;
using ICD.Connect.Telemetry.Providers;

namespace ICD.Connect.Devices.Telemetry.DeviceInfo
{
	public interface INetworkDeviceInfo : ITelemetryProvider
	{
		[EventTelemetry(DeviceTelemetryNames.DEVICE_HOSTNAME_CHANGED)]
		event EventHandler<StringEventArgs> OnHostnameChanged;

		[EventTelemetry(DeviceTelemetryNames.DEVICE_NETWORK_DNS_CHANGED)]
		event EventHandler<StringEventArgs> OnDnsChanged;

		[PropertyTelemetry(DeviceTelemetryNames.DEVICE_HOSTNAME, null, DeviceTelemetryNames.DEVICE_HOSTNAME_CHANGED)]
		string Hostname { get; set; }
		
		[PropertyTelemetry(DeviceTelemetryNames.DEVICE_NETWORK_DNS, null, DeviceTelemetryNames.DEVICE_NETWORK_DNS_CHANGED)]
		string Dns { get; set; }

		[CollectionTelemetry("Adapters")]
		AdapterNetworkDeviceInfoCollection Adapters { get; }
	}

	public interface INetworkDeviceInfo<TAdapterInfo> : INetworkDeviceInfo
		where TAdapterInfo : IAdapterNetworkDeviceInfo
	{
	}
}