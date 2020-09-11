using System;
using System.Collections.Generic;
using ICD.Common.Properties;
using ICD.Common.Utils.Extensions;
using ICD.Common.Logging.LoggingContexts;
using ICD.Common.Utils.Services.Logging;
using ICD.Connect.API.Commands;
using ICD.Connect.API.Nodes;
using ICD.Connect.Devices.EventArguments;

namespace ICD.Connect.Devices.Controls
{
	public abstract class AbstractPowerDeviceControl<TDevice> : AbstractDeviceControl<TDevice>, IPowerDeviceControl
		where TDevice : IDevice
	{
		public delegate void PrePowerDelegate(Action powerCallback);

		public PrePowerDelegate PrePowerOn { get; set; }
		public PrePowerDelegate PrePowerOff { get; set; }

		/// <summary>
		/// Raised when the powered state changes.
		/// </summary>
		public event EventHandler<PowerDeviceControlPowerStateApiEventArgs> OnPowerStateChanged;

		private ePowerState m_PowerState;

		/// <summary>
		/// Gets the powered state of the device.
		/// </summary>
		public ePowerState PowerState
		{
			get { return m_PowerState; }
			protected set { SetPowerState(value, GetExpectedDurationForNewPowerState(m_PowerState)); }
		}

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="parent"></param>
		/// <param name="id"></param>
		protected AbstractPowerDeviceControl(TDevice parent, int id)
			: base(parent, id)
		{
			// Initialize activities
			SetPowerState(ePowerState.Unknown, 0);
		}

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="parent"></param>
		/// <param name="id"></param>
		/// <param name="uuid"></param>
		protected AbstractPowerDeviceControl(TDevice parent, int id, Guid uuid)
			: base(parent, id, uuid)
		{
			// Initialize activities
			SetPowerState(ePowerState.Unknown, 0);
		}

		/// <summary>
		/// Override to release resources.
		/// </summary>
		/// <param name="disposing"></param>
		protected override void DisposeFinal(bool disposing)
		{
			OnPowerStateChanged = null;

			base.DisposeFinal(disposing);
		}

		#region Methods

		/// <summary>
		/// Powers on the device.
		/// </summary>
		[PublicAPI]
		public void PowerOn()
		{
			if (PrePowerOn != null)
				PrePowerOn(PowerOnFinal);
			else
				PowerOnFinal();
		}

		[PublicAPI]
		public void PowerOn(bool bypassPrePowerOn)
		{
			if (bypassPrePowerOn)
				PowerOnFinal();
			else
				PowerOn();
		}

		protected abstract void PowerOnFinal();

		/// <summary>
		/// Powers off the device.
		/// </summary>
		[PublicAPI]
		public void PowerOff()
		{
			if (PrePowerOff != null)
				PrePowerOff(PowerOffFinal);
			else
				PowerOffFinal();
		}

		[PublicAPI]
		public void PowerOff(bool bypassPostPowerOff)
		{
			if (bypassPostPowerOff)
				PowerOffFinal();
			else
				PowerOff();
		}

		protected abstract void PowerOffFinal();

		/// <summary>
		/// Override to implement expected durations for various power states
		/// </summary>
		/// <param name="state">power state to get duration for</param>
		/// <returns>Expected duration in milliseconds</returns>
		protected virtual long GetExpectedDurationForNewPowerState(ePowerState state)
		{
			return 0;
		}

		/// <summary>
		/// Sets the power state, using the given expected duration in the resulting event
		/// </summary>
		/// <param name="powerState"></param>
		/// <param name="expectedDuration"></param>
		protected void SetPowerState(ePowerState powerState, long expectedDuration)
		{
			try
			{
				if (powerState == m_PowerState)
					return;

				m_PowerState = powerState;

				Logger.LogSetTo(eSeverity.Informational, "PowerState", m_PowerState);

				OnPowerStateChanged.Raise(this, new PowerDeviceControlPowerStateApiEventArgs(powerState, expectedDuration));
			}
			finally
			{
				Activities.LogActivity(PowerDeviceControlActivities.GetPowerActivity(m_PowerState));
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

			foreach (IConsoleNodeBase node in PowerDeviceControlConsole.GetConsoleNodes(this))
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

			PowerDeviceControlConsole.BuildConsoleStatus(this, addRow);
		}

		/// <summary>
		/// Gets the child console commands.
		/// </summary>
		/// <returns></returns>
		public override IEnumerable<IConsoleCommand> GetConsoleCommands()
		{
			foreach (IConsoleCommand command in GetBaseConsoleCommands())
				yield return command;

			foreach (IConsoleCommand command in PowerDeviceControlConsole.GetConsoleCommands(this))
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
