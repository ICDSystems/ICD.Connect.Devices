using System.Collections.Generic;
using System.Linq;
using ICD.Connect.Devices.Telemetry.DeviceInfo.NetworkInfo.AdapterInfo;

namespace ICD.Connect.Devices.Telemetry.DeviceInfo.NetworkInfo
{
	public sealed class ConfiguredNetworkDeviceInfoTelemetry : AbstractNetworkDeviceInfoTelemetry<ConfiguredAdapterNetworkDeviceInfoTelemetry>, IConfiguredNetworkDeviceInfoTelemetry
	{


		protected override ConfiguredAdapterNetworkDeviceInfoTelemetry CreateNewAdapter(int address)
		{
			return new ConfiguredAdapterNetworkDeviceInfoTelemetry(address);
		}

		/// <summary>
		/// Apply the configuration from the settings
		/// </summary>
		/// <param name="settings"></param>
		public void ApplySettings(ConfiguredNetworkDeviceInfoTelemetrySettings settings)
		{
			Hostname = settings.Hostname;
			Dns = settings.Dns;
			

			ApplyAdapterSettings(settings.Adapters);
		}

		/// <summary>
		/// Copy the configuration to the given settings
		/// </summary>
		/// <param name="settings"></param>
		public void CopySettings(ConfiguredNetworkDeviceInfoTelemetrySettings settings)
		{
			settings.Hostname = Hostname;
			settings.Dns = Dns;
			
			CopyAdapterSettings(settings.Adapters);
		}

		private void CopyAdapterSettings(List<ConfiguredAdapterNetworkDeviceInfoTelemetrySettings> settings)
		{
			settings.Clear();

			foreach (var adapter in Adapters)
			{
				var adapterSettings = new ConfiguredAdapterNetworkDeviceInfoTelemetrySettings();
				adapter.CopySettings(adapterSettings);
				settings.Add(adapterSettings);
			}
		}

		private void ApplyAdapterSettings(IEnumerable<ConfiguredAdapterNetworkDeviceInfoTelemetrySettings> adaptersSettings)
		{
			AdaptersClear();

			foreach (ConfiguredAdapterNetworkDeviceInfoTelemetrySettings adapterSettings in adaptersSettings)
			{
				ConfiguredAdapterNetworkDeviceInfoTelemetry adapter = GetOrAddAdapter(adapterSettings.Address);
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