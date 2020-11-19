using System;
using System.Collections.Generic;
using ICD.Common.Properties;
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

		event EventHandler OnAdaptersChanged;

		[PropertyTelemetry(DeviceTelemetryNames.DEVICE_HOSTNAME, null, DeviceTelemetryNames.DEVICE_HOSTNAME_CHANGED)]
		string Hostname { get; set; }
		
		[PropertyTelemetry(DeviceTelemetryNames.DEVICE_NETWORK_DNS, null, DeviceTelemetryNames.DEVICE_NETWORK_DNS_CHANGED)]
		string Dns { get; set; }

		[NotNull]
		IAdapterNetworkDeviceInfo GetOrAddAdapter(int address);

	}

	public interface INetworkDeviceInfo<TAdapterInfo> : INetworkDeviceInfo
		where TAdapterInfo : IAdapterNetworkDeviceInfo
	{
		[CollectionTelemetry("Adapters")]
		IEnumerable<TAdapterInfo> Adapters { get; }

		[NotNull]
		new TAdapterInfo GetOrAddAdapter(int address);
	}
}