using System;
using System.Collections.Generic;
using ICD.Common.Properties;
using ICD.Common.Utils.EventArguments;
using ICD.Connect.Devices.Telemetry.DeviceInfo.NetworkInfo.AdapterInfo;
using ICD.Connect.Telemetry.Attributes;
using ICD.Connect.Telemetry.Providers;

namespace ICD.Connect.Devices.Telemetry.DeviceInfo.NetworkInfo
{
	public interface INetworkDeviceInfoTelemetry : ITelemetryProvider
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


		/// <summary>
		/// This identifies the node for telemetry
		/// Hack because we don't have a single-node telemetry option yet
		/// This should be '0' for now
		/// </summary>
		[TelemetryCollectionIdentity]
		string NodeIdentifier { get; }
	}

	public interface INetworkDeviceInfoTelemetry<TAdapterInfo> : INetworkDeviceInfoTelemetry
		where TAdapterInfo : IAdapterNetworkDeviceInfoTelemetry
	{

		[CollectionTelemetry("Adapters")]
		IEnumerable<TAdapterInfo> Adapters { get; }

		[NotNull]
		TAdapterInfo GetOrAddAdapter(int address);
	}
}