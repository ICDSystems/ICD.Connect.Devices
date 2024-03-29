﻿using System;
using System.Collections.Generic;
using ICD.Common.Utils.Extensions;
using ICD.Connect.API;
using ICD.Connect.API.Commands;
using ICD.Connect.API.Info;
using ICD.Connect.API.Nodes;
using ICD.Connect.Devices.Controls.Power;
using ICD.Connect.Devices.EventArguments;
using ICD.Connect.Devices.Proxies.Devices;

namespace ICD.Connect.Devices.Proxies.Controls
{
	public sealed class ProxyPowerDeviceControl : AbstractProxyDeviceControl, IPowerDeviceControl
	{
		public event EventHandler<PowerDeviceControlPowerStateApiEventArgs> OnPowerStateChanged;

		#region Properties

		public ePowerState PowerState { get; private set; }

		/// <summary>
		/// Gets/sets the delegate to execute before powering the device.
		/// TODO - How to handle?
		/// </summary>
		PrePowerDelegate IPowerDeviceControl.PrePowerOn { get; set; }

		/// <summary>
		/// Gets/sets the delegate to execute before powering off the device.
		/// TODO - How to handle?
		/// </summary>
		PrePowerDelegate IPowerDeviceControl.PrePowerOff { get; set; }

		#endregion

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="parent"></param>
		/// <param name="id"></param>
		public ProxyPowerDeviceControl(IProxyDevice parent, int id)
			: base(parent, id)
		{
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
		public void PowerOn()
		{
			CallMethod(PowerDeviceControlApi.METHOD_POWER_ON);
		}

		public void PowerOn(bool bypassPrePowerOn)
		{
			CallMethod(PowerDeviceControlApi.METHOD_POWER_ON_BYPASS, bypassPrePowerOn);
		}

		/// <summary>
		/// Powers off the device.
		/// </summary>
		public void PowerOff()
		{
			CallMethod(PowerDeviceControlApi.METHOD_POWER_OFF);
		}

		public void PowerOff(bool bypassPostPowerOff)
		{
			CallMethod(PowerDeviceControlApi.METHOD_POWER_OFF_BYPASS, bypassPostPowerOff);
		}

		#endregion

		#region API

		/// <summary>
		/// Override to build initialization commands on top of the current class info.
		/// </summary>
		/// <param name="command"></param>
		protected override void Initialize(ApiClassInfo command)
		{
			base.Initialize(command);

			ApiCommandBuilder.UpdateCommand(command)
							 .SubscribeEvent(PowerDeviceControlApi.EVENT_POWER_STATE)
							 .GetProperty(PowerDeviceControlApi.PROPERTY_POWER_STATE)
							 .Complete();
		}

		/// <summary>
		/// Updates the proxy with event feedback info.
		/// </summary>
		/// <param name="name"></param>
		/// <param name="result"></param>
		protected override void ParseEvent(string name, ApiResult result)
		{
			switch (name)
			{
				case PowerDeviceControlApi.EVENT_POWER_STATE:
					HandlePowerStateChangedEvent(result.GetValue<PowerDeviceControlPowerStateEventData>());
					break;
				default:
					base.ParseEvent(name, result);
					break;
			}
		}

		protected override void ParseProperty(string name, ApiResult result)
		{
			switch (name)
			{
				case PowerDeviceControlApi.PROPERTY_POWER_STATE:
					PowerState = result.GetValue<ePowerState>();
					break;
				default:
					base.ParseProperty(name, result);
					break;
			}
		}

		/// <summary>
		/// Handles the power state change event from the remote device
		/// Necessary to preserve the "ExpectedDuration" property
		/// </summary>
		/// <param name="data"></param>
		private void HandlePowerStateChangedEvent(PowerDeviceControlPowerStateEventData data)
		{
			if (UpdatePowerState(data.PowerState))
				OnPowerStateChanged.Raise(this, new PowerDeviceControlPowerStateApiEventArgs(data));
		}

		/// <summary>
		/// Updates the power state, and returns true if it chagned
		/// </summary>
		/// <param name="powerState">New power state</param>
		/// <returns>true if changed, else false</returns>
		private bool UpdatePowerState(ePowerState powerState)
		{
			if (powerState == PowerState)
				return false;

			PowerState = powerState;

			return true;
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
