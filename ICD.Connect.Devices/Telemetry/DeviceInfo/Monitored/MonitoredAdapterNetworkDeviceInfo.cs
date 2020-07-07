using ICD.Connect.Devices.Telemetry.DeviceInfo.Abstract;

namespace ICD.Connect.Devices.Telemetry.DeviceInfo.Monitored
{
	public sealed class MonitoredAdapterNetworkDeviceInfo : AbstractAdapterNetworkDeviceInfo, IMonitoredAdapterNetworkDeviceInfo
	{
		public MonitoredAdapterNetworkDeviceInfo(int address) : base(address)
		{
		}
	}
}