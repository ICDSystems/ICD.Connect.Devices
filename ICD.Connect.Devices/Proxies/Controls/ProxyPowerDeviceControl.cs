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
		public event EventHandler<PowerDeviceControlPowerStateApiEventArgs> OnIsPoweredChanged;

		private bool m_IsPowered;

		/// <summary>
		/// Gets the powered state of the device.
		/// </summary>
		public bool IsPowered
		{
			get { return m_IsPowered; }
			[UsedImplicitly]
			private set
			{
				if (value == m_IsPowered)
					return;

				m_IsPowered = value;

				OnIsPoweredChanged.Raise(this, new PowerDeviceControlPowerStateApiEventArgs(m_IsPowered));
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
			OnIsPoweredChanged = null;

			base.DisposeFinal(disposing);
		}

		/// <summary>
		/// Override to build initialization commands on top of the current class info.
		/// </summary>
		/// <param name="command"></param>
		protected override void Initialize(ApiClassInfo command)
		{
			base.Initialize(command);

			ApiCommandBuilder.UpdateCommand(command)
			                 .SubscribeEvent(PowerDeviceControlApi.EVENT_IS_POWERED)
			                 .GetProperty(PowerDeviceControlApi.PROPERTY_IS_POWERED)
			                 .Complete();
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
