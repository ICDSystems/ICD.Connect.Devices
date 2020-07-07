using System;
using System.Collections.Generic;
using ICD.Common.Logging.LoggingContexts;
using ICD.Common.Properties;
using ICD.Common.Utils.Extensions;
using ICD.Common.Utils.Services.Logging;
using ICD.Connect.API.Commands;
using ICD.Connect.API.Nodes;
using ICD.Connect.Devices.EventArguments;
using ICD.Connect.Devices.Telemetry.DeviceInfo;
using ICD.Connect.Devices.Telemetry.DeviceInfo.Configured;
using ICD.Connect.Devices.Telemetry.DeviceInfo.Monitored;
using ICD.Connect.Settings;
using ICD.Connect.Settings.Originators;

namespace ICD.Connect.Devices
{
	/// <summary>
	/// Base class for devices.
	/// </summary>
	public abstract class AbstractDeviceBase<T> : AbstractOriginator<T>, IDeviceBase
		where T : IDeviceBaseSettings, new()
	{
		/// <summary>
		/// Raised when the device becomes online/offline.
		/// </summary>
		public event EventHandler<DeviceBaseOnlineStateApiEventArgs> OnIsOnlineStateChanged;

		#region Fields

		private bool m_IsOnline;
		private readonly ConfiguredDeviceInfo m_ConfiguredDeviceInfo;
		private readonly MonitoredDeviceInfo m_MonitoredDeviceInfo;

		#endregion

		#region Properties

		/// <summary>
		/// Returns true if the device hardware is detected by the system.
		/// </summary>
		public bool IsOnline
		{
			get { return m_IsOnline; }
			private set
			{
				if (value == m_IsOnline)
					return;

				m_IsOnline = value;

				Logger.LogSetTo(m_IsOnline ? eSeverity.Informational : eSeverity.Error, "IsOnline", m_IsOnline);
				Activities.LogActivity(DeviceBaseActivities.GetIsOnlineActivity(m_IsOnline));

				HandleOnlineStateChange(m_IsOnline);

				OnIsOnlineStateChanged.Raise(this, new DeviceBaseOnlineStateApiEventArgs(IsOnline));
			}
		}

		/// <summary>
		/// Device Info Telemetry, configured from DAV
		/// </summary>
		public IConfiguredDeviceInfo ConfiguredDeviceInfo { get { return m_ConfiguredDeviceInfo; } }

		/// <summary>
		/// Device Info Telemetry, monitored from the device itself
		/// </summary>
		public IMonitoredDeviceInfo MonitoredDeviceInfo { get { return m_MonitoredDeviceInfo; } }

		/// <summary>
		/// Device Info Telemetry, returns both monitored and configured telemetry
		/// </summary>
		public IEnumerable<IDeviceInfo> DeviceInfo
		{
			get
			{
				yield return ConfiguredDeviceInfo;
				yield return MonitoredDeviceInfo;
			}
		}

		#endregion

		public AbstractDeviceBase()
		{
			m_ConfiguredDeviceInfo = new ConfiguredDeviceInfo();
			m_MonitoredDeviceInfo = new MonitoredDeviceInfo();
		}

		#region Private Methods

		/// <summary>
		/// Release resources.
		/// </summary>
		protected override void DisposeFinal(bool disposing)
		{
			OnIsOnlineStateChanged = null;

			base.DisposeFinal(disposing);
		}

		/// <summary>
		/// Gets the current online status of the device.
		/// </summary>
		/// <returns></returns>
		protected abstract bool GetIsOnlineStatus();

		/// <summary>
		/// Override to handle the device going online/offline.
		/// </summary>
		/// <param name="isOnline"></param>
		protected virtual void HandleOnlineStateChange(bool isOnline)
		{
		}

		/// <summary>
		/// Updates the cached online status and raises the OnIsOnlineStateChanged event if the cache changes.
		/// </summary>
		[PublicAPI]
		protected virtual void UpdateCachedOnlineStatus()
		{
			IsOnline = GetIsOnlineStatus();
		}

		#endregion

		#region Settings

		/// <summary>
		/// Override to clear the instance settings.
		/// </summary>
		protected override void ClearSettingsFinal()
		{
			base.ClearSettingsFinal();

			ConfiguredDeviceInfo.ClearSettings();

			UpdateCachedOnlineStatus();
		}

		/// <summary>
		/// Override to apply settings to the instance.
		/// </summary>
		/// <param name="settings"></param>
		/// <param name="factory"></param>
		protected override void ApplySettingsFinal(T settings, IDeviceFactory factory)
		{
			base.ApplySettingsFinal(settings, factory);

			ConfiguredDeviceInfo.ApplySettings(settings.ConfiguredDeviceInfo);

			UpdateCachedOnlineStatus();
		}

		/// <summary>
		/// Override to apply properties to the settings instance.
		/// </summary>
		/// <param name="settings"></param>
		protected override void CopySettingsFinal(T settings)
		{
			base.CopySettingsFinal(settings);

			ConfiguredDeviceInfo.CopySettings(settings.ConfiguredDeviceInfo);
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
		/// Wrokaround for "unverifiable code" warning.
		/// </summary>
		/// <returns></returns>
		private IEnumerable<IConsoleNodeBase> GetBaseConsoleNodes()
		{
			return base.GetConsoleNodes();
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

		#endregion
	}
}
