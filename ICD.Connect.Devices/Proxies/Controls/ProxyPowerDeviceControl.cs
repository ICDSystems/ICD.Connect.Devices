using System;
using System.Collections.Generic;
using ICD.Common.Properties;
using ICD.Common.Utils.Extensions;
using ICD.Connect.API;
using ICD.Connect.API.Commands;
using ICD.Connect.API.Info;
using ICD.Connect.API.Nodes;
using ICD.Connect.Devices.Controls;
using ICD.Connect.Devices.EventArguments;
using ICD.Connect.Devices.Proxies.Devices;

namespace ICD.Connect.Devices.Proxies.Controls
{
	public sealed class ProxyPowerDeviceControl : AbstractProxyDeviceControl, IPowerDeviceControl
	{
		public event EventHandler<PowerDeviceControlPowerStateApiEventArgs> OnPowerStateChanged;

		private ePowerState m_PowerState;

		public ePowerState PowerState
		{
			get { return m_PowerState; }
			[UsedImplicitly]
			private set
			{
				if (value == m_PowerState)
					return;

				m_PowerState = value;

				OnPowerStateChanged.Raise(this, new PowerDeviceControlPowerStateApiEventArgs(m_PowerState));
			}
		}

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="parent"></param>
		/// <param name="id"></param>
		public ProxyPowerDeviceControl(IProxyDeviceBase parent, int id)
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
					PowerState = result.GetValue<ePowerState>();
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
