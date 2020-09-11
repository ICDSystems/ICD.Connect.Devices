using System;
using System.Collections.Generic;
using ICD.Common.Utils.Extensions;
using ICD.Common.Logging.LoggingContexts;
using ICD.Common.Utils.Services.Logging;
using ICD.Connect.API.Commands;
using ICD.Connect.API.Nodes;
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

		private bool m_IsOnline;

		#region Properties

		/// <summary>
		/// Returns true if the device hardware is detected by the system.
		/// </summary>
		public bool IsOnline
		{
			get { return m_IsOnline; }
			private set
			{
				try
				{
					if (value == m_IsOnline)
						return;

					m_IsOnline = value;

					Logger.LogSetTo(eSeverity.Informational, "IsOnline", m_IsOnline);

					HandleOnlineStateChange(m_IsOnline);

					OnIsOnlineStateChanged.Raise(this, new DeviceBaseOnlineStateApiEventArgs(IsOnline));
				}
				finally
				{
					Activities.LogActivity(DeviceBaseActivities.GetIsOnlineActivity(m_IsOnline));
				}
			}
		}

		#endregion

		/// <summary>
		/// Constructor.
		/// </summary>
		protected AbstractSimplDeviceBase()
		{
			// Initialize activities
			SetIsOnline(false);
		}

		#region Methods

		/// <summary>
		/// Sets the online state.
		/// </summary>
		/// <param name="online"></param>
		public void SetIsOnline(bool online)
		{
			IsOnline = online;
		}

		/// <summary>
		/// Override to handle the device going online/offline.
		/// </summary>
		/// <param name="isOnline"></param>
		protected virtual void HandleOnlineStateChange(bool isOnline)
		{
		}

		/// <summary>
		/// Release resources.
		/// </summary>
		protected override void DisposeFinal(bool disposing)
		{
			OnIsOnlineStateChanged = null;

			base.DisposeFinal(disposing);
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
