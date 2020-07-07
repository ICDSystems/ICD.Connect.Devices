using System.Collections.Generic;
using ICD.Connect.Devices.Telemetry.DeviceInfo.Abstract;
using ICD.Connect.Devices.Telemetry.DeviceInfo.Configured.Settings;

namespace ICD.Connect.Devices.Telemetry.DeviceInfo.Configured
{
	public sealed class ConfiguredNetworkDeviceInfo : AbstractNetworkDeviceInfo<ConfiguredAdapterNetworkDeviceInfo>, IConfiguredNetworkDeviceInfo
	{
		protected override ConfiguredAdapterNetworkDeviceInfo CreateNewAdapter(int address)
		{
			return new ConfiguredAdapterNetworkDeviceInfo(address);
		}

		/// <summary>
		/// Apply the configuration from the settings
		/// </summary>
		/// <param name="settings"></param>
		public void ApplySettings(ConfiguredNetworkDeviceInfoSettings settings)
		{
			Hostname = settings.Hostname;
			Dns = settings.Dns;
			
			ApplyAdapterSettings(settings.Adapters);
		}

		/// <summary>
		/// Copy the configuration to the given settings
		/// </summary>
		/// <param name="settings"></param>
		public void CopySettings(ConfiguredNetworkDeviceInfoSettings settings)
		{
			settings.Hostname = Hostname;
			settings.Dns = Dns;
			
			CopyAdapterSettings(settings.Adapters);
		}

		private void CopyAdapterSettings(List<ConfiguredAdapterNetworkDeviceInfoSettings> settings)
		{
			settings.Clear();

			foreach (var adapter in Adapters)
			{
				var adapterSettings = new ConfiguredAdapterNetworkDeviceInfoSettings();
				adapter.CopySettings(adapterSettings);
				settings.Add(adapterSettings);
			}
		}

		private void ApplyAdapterSettings(IEnumerable<ConfiguredAdapterNetworkDeviceInfoSettings> adaptersSettings)
		{
			AdaptersClear();

			foreach (ConfiguredAdapterNetworkDeviceInfoSettings adapterSettings in adaptersSettings)
			{
				ConfiguredAdapterNetworkDeviceInfo adapter = GetOrAddAdapter(adapterSettings.Address);
				adapter.ApplySettings(adapterSettings);
			}
		}

		/// <summary>
		/// Clear the items from settings
		/// </summary>
		public void ClearSettings()
		{
			Hostname = null;
			Dns = null;
			//todo: Dispose Adapters
			AdaptersClear();
		}
	}
}