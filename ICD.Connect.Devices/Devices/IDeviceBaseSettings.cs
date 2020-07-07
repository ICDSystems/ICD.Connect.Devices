using ICD.Connect.Devices.Telemetry.DeviceInfo;
using ICD.Connect.Settings;

namespace ICD.Connect.Devices
{
	public interface IDeviceBaseSettings : ISettings
	{
		ConfiguredDeviceInfoSettings ConfiguredDeviceInfo { get; }
	}
}
