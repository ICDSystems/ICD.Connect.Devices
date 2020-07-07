using System;
using ICD.Common.Utils.EventArguments;
using ICD.Connect.Telemetry.Attributes;
using ICD.Connect.Telemetry.Providers;

namespace ICD.Connect.Devices.Telemetry.DeviceInfo.NetworkInfo.AdapterInfo
{
	public interface IAdapterNetworkDeviceInfoTelemetry : ITelemetryProvider
	{
		[EventTelemetry(DeviceTelemetryNames.DEVICE_NETWORK_ADAPTER_NAME_CHANGED)]
		event EventHandler<StringEventArgs> OnNameChanged;

		[EventTelemetry(DeviceTelemetryNames.DEVICE_MAC_ADDRESS_CHANGED)]
		event EventHandler<StringEventArgs> OnMacAddressChanged;

		[EventTelemetry(DeviceTelemetryNames.DEVICE_DHCP_STATUS_CHANGED)]
		event EventHandler<GenericEventArgs<bool?>> OnDhcpChanged;

		[EventTelemetry(DeviceTelemetryNames.DEVICE_IP_ADDRESS_CHANGED)]
		event EventHandler<StringEventArgs> OnIpv4AddressChanged;

		[EventTelemetry(DeviceTelemetryNames.DEVICE_IP_SUBNET_CHANGED)]
		event EventHandler<StringEventArgs> OnIpv4SubnetMaskChanged;

		[EventTelemetry(DeviceTelemetryNames.DEVICE_IP_GATEWAY_CHANGED)]
		event EventHandler<StringEventArgs> OnIpv4GatewayChanged;

		[PropertyTelemetry(DeviceTelemetryNames.DEVICE_NETWORK_ADAPTER_ADDRESS, null, null)]
		[TelemetryCollectionIdentity]
		int Address { get; }

		[PropertyTelemetry(DeviceTelemetryNames.DEVICE_NETWORK_ADAPTER_NAME, null, DeviceTelemetryNames.DEVICE_NETWORK_ADAPTER_NAME_CHANGED)]
		string Name { get; set; }

		[PropertyTelemetry(DeviceTelemetryNames.DEVICE_MAC_ADDRESS, null,DeviceTelemetryNames.DEVICE_MAC_ADDRESS_CHANGED )]
		string MacAddress { get; set; }

		[PropertyTelemetry(DeviceTelemetryNames.DEVICE_DHCP_STATUS, null, DeviceTelemetryNames.DEVICE_DHCP_STATUS_CHANGED)]
		bool? Dhcp { get; set; }

		[PropertyTelemetry(DeviceTelemetryNames.DEVICE_IP_ADDRESS, null, DeviceTelemetryNames.DEVICE_IP_ADDRESS_CHANGED)]
		string Ipv4Address { get; set; }

		[PropertyTelemetry(DeviceTelemetryNames.DEVICE_IP_SUBNET, null, DeviceTelemetryNames.DEVICE_IP_SUBNET_CHANGED)]
		string Ipv4SubnetMask { get; set; }

		[PropertyTelemetry(DeviceTelemetryNames.DEVICE_IP_GATEWAY, null, DeviceTelemetryNames.DEVICE_IP_GATEWAY_CHANGED)]
		string Ipv4Gateway { get; set; }
	
	}
}