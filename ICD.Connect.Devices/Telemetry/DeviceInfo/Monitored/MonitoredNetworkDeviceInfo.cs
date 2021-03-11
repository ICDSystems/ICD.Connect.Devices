using ICD.Connect.Devices.Telemetry.DeviceInfo.Abstract;

namespace ICD.Connect.Devices.Telemetry.DeviceInfo.Monitored
{
	public sealed class MonitoredNetworkDeviceInfo : AbstractNetworkDeviceInfo<MonitoredAdapterNetworkDeviceInfo>, IMonitoredNetworkDeviceInfo
	{
		protected override IAdapterNetworkDeviceInfo CreateNewAdapter(int address)
		{
			return new MonitoredAdapterNetworkDeviceInfo(address);
		}
	}
}