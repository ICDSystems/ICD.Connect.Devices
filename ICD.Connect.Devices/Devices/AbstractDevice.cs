using System;
using System.Collections.Generic;
using ICD.Common.Logging.LoggingContexts;
using ICD.Common.Utils.Extensions;
using ICD.Common.Utils.Services.Logging;
using ICD.Connect.API.Commands;
using ICD.Connect.API.Nodes;
using ICD.Connect.Devices.Controls;
using ICD.Connect.Devices.EventArguments;
using ICD.Connect.Devices.Telemetry.DeviceInfo;
using ICD.Connect.Devices.Telemetry.DeviceInfo.Configured;
using ICD.Connect.Devices.Telemetry.DeviceInfo.Monitored;
using ICD.Connect.Settings;

namespace ICD.Connect.Devices
{
	/// <summary>
	/// Base class for devices.
	/// </summary>
	public abstract class AbstractDevice<T> : AbstractDeviceBase<T>, IDevice
		where T : IDeviceSettings, new()
	{      
		/// <summary>
		/// Raised when ControlsAvailable changes
		/// </summary>
		public event EventHandler<DeviceBaseControlsAvailableApiEventArgs> OnControlsAvailableChanged;

		private readonly DeviceControlsCollection m_Controls;

		private bool m_ControlsAvailable;

		private readonly ConfiguredDeviceInfo m_ConfiguredDeviceInfo;
		private readonly MonitoredDeviceInfo m_MonitoredDeviceInfo;

		#region Properties

		/// <summary>
		/// Gets the category for this originator type (e.g. Device, Port, etc)
		/// </summary>
		public override string Category { get { return "Device"; } }

		/// <summary>
		/// Gets the controls for this device.
		/// </summary>
		public DeviceControlsCollection Controls { get { return m_Controls; } }

		/// <summary>
		/// Gets if controls are available
		/// </summary>
		public bool ControlsAvailable
		{
			get { return m_ControlsAvailable; }
			private set
			{
				if (value == m_ControlsAvailable)
					return;

				m_ControlsAvailable = value;

				Logger.LogSetTo(eSeverity.Informational, "ControlsAvailable", m_ControlsAvailable);

				OnControlsAvailableChanged.Raise(this, new DeviceBaseControlsAvailableApiEventArgs(ControlsAvailable));
			}
		}

		/// <summary>
		/// Gets the discovered model.
		/// Legacy - Hooks MonitoredDeviceInfo for Convinence
		/// </summary>
		[Obsolete("Use MonitoredDeviceInfo Instead")]
		protected string Model
		{
			get { return MonitoredDeviceInfo.Model; }
			set { MonitoredDeviceInfo.Model = value; }
		}

		/// <summary>
		/// Gets the discovered serial number.
		/// Legacy - Hooks MonitoredDeviceInfo for Convinence
		/// </summary>
		[Obsolete("Use MonitoredDeviceInfo Instead")]
		protected string SerialNumber
		{
			get { return MonitoredDeviceInfo.SerialNumber; }
			set { MonitoredDeviceInfo.SerialNumber = value; }
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

		/// <summary>
		/// Constructor.
		/// </summary>
		protected AbstractDevice()
		{
			m_Controls = new DeviceControlsCollection();
			m_ConfiguredDeviceInfo = new ConfiguredDeviceInfo();
			m_MonitoredDeviceInfo = new MonitoredDeviceInfo();
		}

		#region Private Methods

		/// <summary>
		/// Release resources.
		/// </summary>
		protected override void DisposeFinal(bool disposing)
		{
			OnControlsAvailableChanged = null;

			Controls.Dispose();

			base.DisposeFinal(disposing);
		}

		/// <summary>
		/// Gets the current state of Control Availability
		/// Default implementation is to follow IsOnline;
		/// </summary>
		/// <returns></returns>
		protected virtual bool GetControlsAvailable()
		{
			return IsOnline;
		}

		/// <summary>
		/// Override to handle the device going online/offline.
		/// </summary>
		/// <param name="isOnline"></param>
		protected override void HandleOnlineStateChange(bool isOnline)
		{
			base.HandleOnlineStateChange(isOnline);

			UpdateCachedControlsAvailable();
		}

		/// <summary>
		/// Updates the cached ControlsAvailable status and raises the OnControlsAvailableChanged if the cache chagnes
		/// </summary>
		protected virtual void UpdateCachedControlsAvailable()
		{
			ControlsAvailable = GetControlsAvailable();
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

			Controls.Clear();
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

			AddControls(settings, factory, Controls.Add);
		}

		/// <summary>
		/// Override to add controls to the device.
		/// </summary>
		/// <param name="settings"></param>
		/// <param name="factory"></param>
		/// <param name="addControl"></param>
		protected virtual void AddControls(T settings, IDeviceFactory factory, Action<IDeviceControl> addControl)
		{
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

			foreach (IConsoleNodeBase node in DeviceConsole.GetConsoleNodes(this))
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

			DeviceConsole.BuildConsoleStatus(this, addRow);
		}

		/// <summary>
		/// Gets the child console commands.
		/// </summary>
		/// <returns></returns>
		public override IEnumerable<IConsoleCommand> GetConsoleCommands()
		{
			foreach (IConsoleCommand command in GetBaseConsoleCommands())
				yield return command;

			foreach (IConsoleCommand command in DeviceConsole.GetConsoleCommands(this))
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
