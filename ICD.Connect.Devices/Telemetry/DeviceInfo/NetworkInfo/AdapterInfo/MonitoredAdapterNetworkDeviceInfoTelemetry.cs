namespace ICD.Connect.Devices.Telemetry.DeviceInfo.NetworkInfo.AdapterInfo
{
	public sealed class MonitoredAdapterNetworkDeviceInfoTelemetry : AbstractAdapterNetworkDeviceInfoTelemetry, IMonitoredAdapterNetworkDeviceInfoTelemetry
	{
		public MonitoredAdapterNetworkDeviceInfoTelemetry(int address) : base(address)
		{
		}
	}
}