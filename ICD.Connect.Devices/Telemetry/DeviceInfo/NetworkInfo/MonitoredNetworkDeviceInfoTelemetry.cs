using ICD.Connect.Devices.Telemetry.DeviceInfo.NetworkInfo.AdapterInfo;

namespace ICD.Connect.Devices.Telemetry.DeviceInfo.NetworkInfo
{
	public sealed class MonitoredNetworkDeviceInfoTelemetry : AbstractNetworkDeviceInfoTelemetry<MonitoredAdapterNetworkDeviceInfoTelemetry>, IMonitoredNetworkDeviceInfoTelemetry
	{
		protected override MonitoredAdapterNetworkDeviceInfoTelemetry CreateNewAdapter(int address)
		{
			return new MonitoredAdapterNetworkDeviceInfoTelemetry(address);
		}
	}
}