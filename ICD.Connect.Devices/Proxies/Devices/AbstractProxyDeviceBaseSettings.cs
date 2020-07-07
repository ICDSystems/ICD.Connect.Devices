using ICD.Connect.Devices.Telemetry.DeviceInfo;
using ICD.Connect.Devices.Telemetry.DeviceInfo.Configured.Settings;
using ICD.Connect.Settings.Proxies;

namespace ICD.Connect.Devices.Proxies.Devices
{
	public abstract class AbstractProxyDeviceBaseSettings : AbstractProxySettings, IProxyDeviceBaseSettings
	{
		private readonly ConfiguredDeviceInfoSettings m_ConfiguredDeviceInfo;

		public ConfiguredDeviceInfoSettings ConfiguredDeviceInfo { get { return m_ConfiguredDeviceInfo; } }

		protected AbstractProxyDeviceBaseSettings()
		{
			m_ConfiguredDeviceInfo = new ConfiguredDeviceInfoSettings();
		}
	}
}
