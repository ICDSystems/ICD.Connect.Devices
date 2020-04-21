using System;
using System.Collections.Generic;
using System.Linq;
using ICD.Common.Properties;
using ICD.Common.Utils;
using ICD.Common.Utils.Extensions;
using ICD.Common.Utils.Services.Logging;
using ICD.Connect.API;
using ICD.Connect.API.Commands;
using ICD.Connect.API.Info;
using ICD.Connect.API.Nodes;
using ICD.Connect.API.Proxies;
using ICD.Connect.Devices.Controls;
using ICD.Connect.Devices.EventArguments;
using ICD.Connect.Devices.Proxies.Controls;
using ICD.Connect.Devices.Proxies.Devices;

namespace ICD.Connect.Devices.Simpl
{
	public abstract class AbstractSimplProxyDevice<TSettings> : AbstractSimplProxyDeviceBase<TSettings>, ISimplProxyDevice
		where TSettings : IProxyDeviceBaseSettings
	{
		public event EventHandler<DeviceBaseControlsAvailableApiEventArgs> OnControlsAvailableChanged;

		private readonly DeviceControlsCollection m_Controls;
		private readonly SafeCriticalSection m_CriticalSection;
		private readonly Dictionary<IProxy, Func<ApiClassInfo, ApiClassInfo>> m_ProxyBuildCommand;

		private bool m_ControlsAvailable;

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
			set
			{
				if (value == m_ControlsAvailable)
					return;

				m_ControlsAvailable = value;

				Logger.Set("Controls Available", eSeverity.Informational, ControlsAvailable);

				OnControlsAvailableChanged.Raise(this, new DeviceBaseControlsAvailableApiEventArgs(ControlsAvailable));
			}
		}

		/// <summary>
		/// Gets/sets the manufacturer for this device.
		/// </summary>
		public string Manufacturer { get; set; }

		/// <summary>
		/// Gets/sets the model number for this device.
		/// </summary>
		public string Model { get; set; }

		/// <summary>
		/// Gets/sets the serial number for this device.
		/// </summary>
		public string SerialNumber { get; set; }

		/// <summary>
		/// Gets/sets the purchase date for this device.
		/// </summary>
		public DateTime PurchaseDate { get; set; }

		#endregion

		/// <summary>
		/// Constructor.
		/// </summary>
		protected AbstractSimplProxyDevice()
		{
			m_ProxyBuildCommand = new Dictionary<IProxy, Func<ApiClassInfo, ApiClassInfo>>();
			m_CriticalSection = new SafeCriticalSection();
			m_Controls = new DeviceControlsCollection();
		}

		/// <summary>
		/// Override to release resources.
		/// </summary>
		/// <param name="disposing"></param>
		protected override void DisposeFinal(bool disposing)
		{
			OnControlsAvailableChanged = null;

			base.DisposeFinal(disposing);

			DisposeControls();
		}

		private void DisposeControls()
		{
			foreach (IProxyDeviceControl control in m_Controls.OfType<IProxyDeviceControl>())
			{
				DeinitializeProxyControl(control);
				control.Dispose();
			}

			m_Controls.Dispose();
		}

		#region Private Methods

		/// <summary>
		/// Override to build initialization commands on top of the current class info.
		/// </summary>
		/// <param name="command"></param>
		protected override void Initialize(ApiClassInfo command)
		{
			base.Initialize(command);

			ApiCommandBuilder.UpdateCommand(command)
			                 .SubscribeEvent(DeviceBaseApi.EVENT_CONTROLS_AVAILABLE)
			                 .GetProperty(DeviceBaseApi.PROPERTY_CONTROLS_AVAILABLE)
			                 .GetNodeGroup(DeviceBaseApi.NODE_GROUP_CONTROLS)
			                 .Complete();
		}

		/// <summary>
		/// Instructs the proxy to clear any initialized values.
		/// </summary>
		public override void Deinitialize()
		{
			base.Deinitialize();

			foreach (IProxy control in m_CriticalSection.Execute(() => m_ProxyBuildCommand.Keys.ToArray()))
				DeinitializeProxyControl(control);
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
				case DeviceBaseApi.EVENT_CONTROLS_AVAILABLE:
					ControlsAvailable = result.GetValue<bool>();
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
				case DeviceBaseApi.PROPERTY_CONTROLS_AVAILABLE:
					ControlsAvailable = result.GetValue<bool>();
					break;
			}
		}

		/// <summary>
		/// Updates the proxy with a node group result.
		/// </summary>
		/// <param name="name"></param>
		/// <param name="nodeGroup"></param>
		protected override void ParseNodeGroup(string name, ApiNodeGroupInfo nodeGroup)
		{
			base.ParseNodeGroup(name, nodeGroup);

			switch (name)
			{
				case DeviceBaseApi.NODE_GROUP_CONTROLS:
					foreach (ApiNodeGroupKeyInfo item in nodeGroup)
					{
						IProxyDeviceControl proxy = InitializeProxyControl((int)item.Key);
						if (proxy != null)
							proxy.ParseInfo(item.Node);
					}
					break;
			}
		}

		/// <summary>
		/// Gets the proxy control with the given id.
		/// Subscribes and initializes if this is the first time accessing the control.
		/// </summary>
		/// <param name="id"></param>
		[CanBeNull]
		private IProxyDeviceControl InitializeProxyControl(int id)
		{
			IDeviceControl control;
			if (!m_Controls.TryGetControl(id, out control))
			{
				IcdConsole.PrintLine(eConsoleColor.Blue, "InitializeProxyControl: {0} has no control at id {1}", this, id);
				return null;
			}

			IProxyDeviceControl proxyControl = control as IProxyDeviceControl;
			if (proxyControl == null)
			{
				IcdConsole.PrintLine(eConsoleColor.Blue, "InitializeProxyControl: {0} control at id {1} is a not a proxy", this, id);
				return null;
			}

			m_CriticalSection.Enter();

			try
			{
				if (m_ProxyBuildCommand.ContainsKey(proxyControl))
					return proxyControl;

				// Build the root command
				Func<ApiClassInfo, ApiClassInfo> buildCommand = local =>
				                                                ApiCommandBuilder.NewCommand()
				                                                                 .AtNodeGroup("Controls")
				                                                                 .AddKey((uint)id, local)
				                                                                 .Complete();

				m_ProxyBuildCommand.Add(proxyControl, buildCommand);

				// Start handling the proxy callbacks
				Subscribe(proxyControl);
			}
			finally
			{
				m_CriticalSection.Leave();
			}

			// Initialize the proxy
			IcdConsole.PrintLine(eConsoleColor.Blue, "InitializeProxyControl: {0} initializing proxy control {1}", this, id);
			proxyControl.Initialize();

			return proxyControl;
		}

		private void DeinitializeProxyControl(IProxy control)
		{
			if (control == null)
				throw new ArgumentNullException("control");

			if (!m_CriticalSection.Execute(() => m_ProxyBuildCommand.Remove(control)))
				return;

			// Initialize the proxy
			IcdConsole.PrintLine(eConsoleColor.Blue, "DeinitializeProxyControl: {0} deinitializing proxy control {1}", this, control);
			Unsubscribe(control);
			control.Deinitialize();
		}

		#endregion

		#region Proxy Callbacks

		/// <summary>
		/// Subscribe to the proxy events.
		/// </summary>
		/// <param name="control"></param>
		private void Subscribe(IProxy control)
		{
			control.OnCommand += ProxyControlOnCommand;
		}

		/// <summary>
		/// Unsubscribe from the proxy events.
		/// </summary>
		/// <param name="control"></param>
		private void Unsubscribe(IProxy control)
		{
			control.OnCommand -= ProxyControlOnCommand;
		}

		/// <summary>
		/// Called when a proxy raises a command to be sent to the remote API.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="eventArgs"></param>
		private void ProxyControlOnCommand(object sender, ApiClassInfoEventArgs eventArgs)
		{
			IProxy proxy = sender as IProxy;
			if (proxy == null)
				return;

			// Build the full command from the device to the proxy
			ApiClassInfo command = m_ProxyBuildCommand[proxy](eventArgs.Data);

			SendCommand(command);
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