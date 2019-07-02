﻿using System;
using System.Collections.Generic;
using ICD.Common.Properties;
using ICD.Common.Utils.Extensions;
using ICD.Common.Utils.Services.Logging;
using ICD.Connect.API.Commands;
using ICD.Connect.API.Nodes;
using ICD.Connect.Devices.EventArguments;

namespace ICD.Connect.Devices.Controls
{
	public abstract class AbstractPowerDeviceControl<TDevice> : AbstractDeviceControl<TDevice>, IPowerDeviceControl
		where TDevice : IDeviceBase
	{
		public delegate void PrePowerOnDelegate();

		public delegate void PostPowerOffDelegate();

		public PrePowerOnDelegate   PrePowerOn   { get; set; }
		public PostPowerOffDelegate PostPowerOff { get; set; }
		
		/// <summary>
		/// Raised when the powered state changes.
		/// </summary>
		public event EventHandler<PowerDeviceControlPowerStateApiEventArgs> OnIsPoweredChanged;

		private bool m_IsPowered;

		/// <summary>
		/// Gets the powered state of the device.
		/// </summary>
		public bool IsPowered
		{
			get { return m_IsPowered; }
			protected set
			{
				if (value == m_IsPowered)
					return;

				m_IsPowered = value;

				Log(eSeverity.Informational, "IsPowered set to {0}", m_IsPowered);

				OnIsPoweredChanged.Raise(this, new PowerDeviceControlPowerStateApiEventArgs(m_IsPowered));
			}
		}

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="parent"></param>
		/// <param name="id"></param>
		protected AbstractPowerDeviceControl(TDevice parent, int id)
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

		#region Methods

		/// <summary>
		/// Powers on the device.
		/// </summary>
		[PublicAPI]
		public void PowerOn()
		{
			if(PrePowerOn != null)
				PrePowerOn.Invoke();
			else
				PowerOnFinal();
		}
		
		[PublicAPI]
		public void PowerOn(bool bypassPrePowerOn)
		{
			if(bypassPrePowerOn)
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
			PowerOffFinal();
			if(PostPowerOff != null)
				PostPowerOff.Invoke();
		}

		[PublicAPI]
		public void PowerOff(bool bypassPostPowerOff)
		{
			if(bypassPostPowerOff)
				PowerOffFinal();
			else
				PowerOff();
		}

		protected abstract void PowerOffFinal();
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
