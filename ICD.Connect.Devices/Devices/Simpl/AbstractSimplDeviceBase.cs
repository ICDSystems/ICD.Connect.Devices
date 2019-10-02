using System;
using System.Collections.Generic;
using ICD.Common.Utils.Extensions;
using ICD.Common.Utils.Services.Logging;
using ICD.Connect.API.Commands;
using ICD.Connect.API.Nodes;
using ICD.Connect.Devices.Controls;
using ICD.Connect.Devices.EventArguments;
using ICD.Connect.Settings.Originators.Simpl;

namespace ICD.Connect.Devices.Simpl
{
	public abstract class AbstractSimplDeviceBase<TSettings> : AbstractSimplOriginator<TSettings>, ISimplDeviceBase
		where TSettings : ISimplDeviceBaseSettings, new()
	{
		/// <summary>
		/// Raised when the device goes online/offline.
		/// </summary>
		public event EventHandler<DeviceBaseOnlineStateApiEventArgs> OnIsOnlineStateChanged;

		/// <summary>
		/// Raised when ControlsAvailable changes
		/// </summary>
		public event EventHandler<DeviceBaseControlsAvailableApiEventArgs> OnControlsAvailableChanged;

		private readonly DeviceControlsCollection m_Controls;

		private bool m_IsOnline;

		private bool m_ControlsAvailable;

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

				Log(eSeverity.Informational, "Online status changed to {0}", IsOnline);

				OnIsOnlineStateChanged.Raise(this, new DeviceBaseOnlineStateApiEventArgs(IsOnline));
			}
		}

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

				Log(eSeverity.Informational, "Controls Available changed to {0}", ControlsAvailable);

				OnControlsAvailableChanged.Raise(this, new DeviceBaseControlsAvailableApiEventArgs(ControlsAvailable));
			}
		}

		/// <summary>
		/// Gets the controls for this device.
		/// </summary>
		public DeviceControlsCollection Controls { get { return m_Controls; } }

		/// <summary>
		/// Constructor.
		/// </summary>
		protected AbstractSimplDeviceBase()
		{
			m_Controls = new DeviceControlsCollection();

			Name = GetType().Name;
		}

		public void SetIsOnline(bool online)
		{
			IsOnline = online;
		}

		/// <summary>
		/// Gets the current state of Control Avaliability
		/// Default implementation is to follow IsOnline;
		/// </summary>
		/// <returns></returns>
		protected virtual bool GetControlsAvailable()
		{
			return IsOnline;
		}

		/// <summary>
		/// Updates the cached ControlsAvailable status and raises the OnControlsAvailableChanged if the cache chagnes
		/// </summary>
		protected virtual void UpdateCachedControlsAvailable()
		{
			ControlsAvailable = GetControlsAvailable();
		}

		/// <summary>
		/// Release resources.
		/// </summary>
		protected override void DisposeFinal(bool disposing)
		{
			OnIsOnlineStateChanged = null;

			m_Controls.Dispose();

			base.DisposeFinal(disposing);
		}

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
