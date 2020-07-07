using System;
using System.Collections.Generic;
using ICD.Common.Properties;
using ICD.Common.Utils.Extensions;
using ICD.Common.Logging.LoggingContexts;
using ICD.Common.Utils.Services.Logging;
using ICD.Connect.API;
using ICD.Connect.API.Commands;
using ICD.Connect.API.Info;
using ICD.Connect.API.Nodes;
using ICD.Connect.Devices.EventArguments;
using ICD.Connect.Devices.Telemetry.DeviceInfo;
using ICD.Connect.Settings;
using ICD.Connect.Settings.Proxies;

namespace ICD.Connect.Devices.Proxies.Devices
{
	public abstract class AbstractProxyDeviceBase<TSettings> : AbstractProxyOriginator<TSettings>, IProxyDeviceBase
		where TSettings : IProxyDeviceBaseSettings
	{
		public event EventHandler<DeviceBaseOnlineStateApiEventArgs> OnIsOnlineStateChanged;

		#region Fields

		private bool m_IsOnline;
		private readonly ConfiguredDeviceInfoTelemetry m_ConfiguredDeviceInfo;
		private readonly MonitoredDeviceInfoTelemetry m_MonitoredDeviceInfo;

		#endregion

		#region Properties

		/// <summary>
		/// Gets the online state for the device.
		/// </summary>
		public bool IsOnline
		{
			get { return m_IsOnline; }
			[UsedImplicitly]
			private set
			{
				if (value == m_IsOnline)
					return;

				m_IsOnline = value;

				Logger.LogSetTo(eSeverity.Informational, "IsOnline", m_IsOnline);
				Activities.LogActivity(DeviceBaseActivities.GetIsOnlineActivity(m_IsOnline));

				OnIsOnlineStateChanged.Raise(this, new DeviceBaseOnlineStateApiEventArgs(m_IsOnline));
			}
		}

		/// <summary>
		/// Device Info Telemetry, configured from DAV
		/// Todo: Make this work over proxy?
		/// </summary>
		public IConfiguredDeviceInfoTelemetry ConfiguredDeviceInfo { get { return m_ConfiguredDeviceInfo; } }

		/// <summary>
		/// Device Info Telemetry, monitored from the device itself
		/// Todo: Make this work over proxy?
		/// </summary>
		public IMonitoredDeviceInfoTelemetry MonitoredDeviceInfo { get { return m_MonitoredDeviceInfo; } }

		#endregion

		protected AbstractProxyDeviceBase()
		{
			m_ConfiguredDeviceInfo = new ConfiguredDeviceInfoTelemetry();
			m_MonitoredDeviceInfo = new MonitoredDeviceInfoTelemetry();
		}

		/// <summary>
		/// Override to release resources.
		/// </summary>
		/// <param name="disposing"></param>
		protected override void DisposeFinal(bool disposing)
		{
			OnIsOnlineStateChanged = null;

			base.DisposeFinal(disposing);
		}

		#region Settings

		/// <summary>
		/// Override to clear the instance settings.
		/// </summary>
		protected override void ClearSettingsFinal()
		{
			base.ClearSettingsFinal();

			ConfiguredDeviceInfo.ClearSettings();
		}

		/// <summary>
		/// Override to apply settings to the instance.
		/// </summary>
		/// <param name="settings"></param>
		/// <param name="factory"></param>
		protected override void ApplySettingsFinal(TSettings settings, IDeviceFactory factory)
		{
			base.ApplySettingsFinal(settings, factory);

			ConfiguredDeviceInfo.ApplySettings(settings.ConfiguredDeviceInfo);
		}

		/// <summary>
		/// Override to apply properties to the settings instance.
		/// </summary>
		/// <param name="settings"></param>
		protected override void CopySettingsFinal(TSettings settings)
		{
			base.CopySettingsFinal(settings);

			ConfiguredDeviceInfo.CopySettings(settings.ConfiguredDeviceInfo);
		}

		#endregion

		#region Private Methods

		/// <summary>
		/// Override to build initialization commands on top of the current class info.
		/// </summary>
		/// <param name="command"></param>
		protected override void Initialize(ApiClassInfo command)
		{
			base.Initialize(command);

			ApiCommandBuilder.UpdateCommand(command)
			                 .SubscribeEvent(DeviceBaseApi.EVENT_IS_ONLINE)
			                 .GetProperty(DeviceBaseApi.PROPERTY_IS_ONLINE)
			                 .Complete();
		}

		/// <summary>
		/// Updates the proxy with event feedback info.
		/// </summary>
		/// <param name="name"></param>
		/// <param name="result"></param>
		protected override void ParseEvent(string name, ApiResult result)
		{
			base.ParseEvent(name, result);

			switch (name)
			{
				case DeviceBaseApi.EVENT_IS_ONLINE:
					IsOnline = result.GetValue<bool>();
					break;
			}
		}

		/// <summary>
		/// Updates the proxy with a property result.
		/// </summary>
		/// <param name="name"></param>
		/// <param name="result"></param>
		protected override void ParseProperty(string name, ApiResult result)
		{
			base.ParseProperty(name, result);

			switch (name)
			{
				case DeviceBaseApi.PROPERTY_IS_ONLINE:
					IsOnline = result.GetValue<bool>();
					break;
			}
		}

		#endregion

		#region Console

		/// <summary>
		/// Gets the child console nodes.
		/// </summary>
		/// <returns></returns>
		public override IEnumerable<IConsoleNodeBase> GetConsoleNodes()
		{
			foreach (IConsoleNodeBase node in GetBaseConsoleNodes())
				yield return node;

			foreach (IConsoleNodeBase node in DeviceBaseConsole.GetConsoleNodes(this))
				yield return node;
		}

		/// <summary>
		/// Calls the delegate for each console status item.
		/// </summary>
		/// <param name="addRow"></param>
		public override void BuildConsoleStatus(AddStatusRowDelegate addRow)
		{
			base.BuildConsoleStatus(addRow);

			DeviceBaseConsole.BuildConsoleStatus(this, addRow);
		}

		/// <summary>
		/// Gets the child console commands.
		/// </summary>
		/// <returns></returns>
		public override IEnumerable<IConsoleCommand> GetConsoleCommands()
		{
			foreach (IConsoleCommand command in GetBaseConsoleCommands())
				yield return command;

			foreach (IConsoleCommand command in DeviceBaseConsole.GetConsoleCommands(this))
				yield return command;
		}

		/// <summary>
		/// Workaround for "unverifiable code" warning.
		/// </summary>
		/// <returns></returns>
		private IEnumerable<IConsoleCommand> GetBaseConsoleCommands()
		{
			return base.GetConsoleCommands();
		}

		/// <summary>
		/// Workaround for "unverifiable code" warning.
		/// </summary>
		/// <returns></returns>
		private IEnumerable<IConsoleNodeBase> GetBaseConsoleNodes()
		{
			return base.GetConsoleNodes();
		}

		#endregion
	}
}
