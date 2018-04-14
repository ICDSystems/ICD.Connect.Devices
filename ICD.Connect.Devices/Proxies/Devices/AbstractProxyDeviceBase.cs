using System;
using System.Collections.Generic;
using System.Linq;
using ICD.Common.Properties;
using ICD.Common.Utils;
using ICD.Common.Utils.Extensions;
using ICD.Connect.API;
using ICD.Connect.API.Commands;
using ICD.Connect.API.Info;
using ICD.Connect.API.Nodes;
using ICD.Connect.API.Proxies;
using ICD.Connect.Devices.Controls;
using ICD.Connect.Devices.EventArguments;
using ICD.Connect.Devices.Proxies.Controls;
using ICD.Connect.Settings.Proxies;

namespace ICD.Connect.Devices.Proxies.Devices
{
	public abstract class AbstractProxyDeviceBase : AbstractProxyOriginator, IProxyDeviceBase
	{
		public event EventHandler<DeviceBaseOnlineStateApiEventArgs> OnIsOnlineStateChanged;

		private readonly DeviceControlsCollection m_Controls;
		private readonly Dictionary<IProxy, Func<ApiClassInfo, ApiClassInfo>> m_ProxyBuildCommand;

		private bool m_IsOnline;

		#region Properties

		/// <summary>
		/// Gets the online state for the device.
		/// </summary>
		public bool IsOnline
		{
			get { return m_IsOnline; }
			[UsedImplicitly]
			private set
			{
				if (value == m_IsOnline)
					return;

				m_IsOnline = value;

				OnIsOnlineStateChanged.Raise(this, new DeviceBaseOnlineStateApiEventArgs(m_IsOnline));
			}
		}

		/// <summary>
		/// Gets the controls for this device.
		/// </summary>
		public DeviceControlsCollection Controls { get { return m_Controls; } }

		#endregion

		/// <summary>
		/// Constructor.
		/// </summary>
		protected AbstractProxyDeviceBase()
		{
			m_ProxyBuildCommand = new Dictionary<IProxy, Func<ApiClassInfo, ApiClassInfo>>();
			m_Controls = new DeviceControlsCollection();
		}

		/// <summary>
		/// Override to release resources.
		/// </summary>
		/// <param name="disposing"></param>
		protected override void DisposeFinal(bool disposing)
		{
			OnIsOnlineStateChanged = null;

			base.DisposeFinal(disposing);

			DisposeControls();
		}

		private void DisposeControls()
		{
			foreach (IProxyDeviceControl control in m_Controls.OfType<IProxyDeviceControl>())
			{
				Unsubscribe(control);
				control.Dispose();
			}

			m_Controls.Dispose();
			m_ProxyBuildCommand.Clear();
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
			                 .SubscribeEvent(DeviceBaseApi.EVENT_IS_ONLINE)
			                 .GetProperty(DeviceBaseApi.PROPERTY_IS_ONLINE)
			                 .GetNodeGroup(DeviceBaseApi.NODE_GROUP_CONTROLS)
			                 .Complete();
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
				case DeviceBaseApi.EVENT_IS_ONLINE:
					IsOnline = result.GetValue<bool>();
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
				case DeviceBaseApi.PROPERTY_IS_ONLINE:
					IsOnline = result.GetValue<bool>();
					break;
			}
		}

		/// <summary>
		/// Updates the proxy with a node group result.
		/// </summary>
		/// <param name="name"></param>
		/// <param name="result"></param>
		protected override void ParseNodeGroup(string name, ApiResult result)
		{
			base.ParseNodeGroup(name, result);

			switch (name)
			{
				case DeviceBaseApi.NODE_GROUP_CONTROLS:
					ApiNodeGroupInfo nodeGroup = result.GetValue<ApiNodeGroupInfo>();
					foreach (ApiNodeGroupKeyInfo item in nodeGroup)
					{
						ApiClassInfo node = item.Node;
						IProxyDeviceControl proxy = LazyLoadProxyControl("Controls", (int)item.Key, node);
						proxy.ParseInfo(node);
					}
					break;
			}
		}

		/// <summary>
		/// Creates a proxy control for the given class info if a control with the given id does not exist.
		/// </summary>
		/// <param name="group"></param>
		/// <param name="id"></param>
		/// <param name="classInfo"></param>
		private IProxyDeviceControl LazyLoadProxyControl(string group, int id, ApiClassInfo classInfo)
		{
			if (m_Controls.Contains(id))
				return m_Controls.GetControl<IProxyDeviceControl>(id);

			Type proxyType = classInfo.GetProxyTypes().FirstOrDefault();
			if (proxyType == null)
				throw new InvalidOperationException(string.Format("No proxy type discovered for control {0}", id));

			// Build the control
			IProxyDeviceControl control = ReflectionUtils.CreateInstance(proxyType, this, id) as IProxyDeviceControl;
			if (control == null)
				throw new InvalidOperationException();

			// Build the root command
			Func<ApiClassInfo, ApiClassInfo> buildCommand = local =>
				                                                ApiCommandBuilder.NewCommand()
				                                                                 .AtNodeGroup(group)
				                                                                 .AddKey((uint)id, local)
				                                                                 .Complete();

			m_ProxyBuildCommand.Add(control, buildCommand);
			m_Controls.Add(control);

			// Start handling the proxy callbacks
			Subscribe(control);

			// Initialize the proxy
			control.Initialize();

			return control;
		}

		#endregion

		#region Proxy Callbacks

		/// <summary>
		/// Subscribe to the proxy events.
		/// </summary>
		/// <param name="originator"></param>
		private void Subscribe(IProxy originator)
		{
			originator.OnCommand += ProxyOnCommand;
		}

		/// <summary>
		/// Unsubscribe from the proxy events.
		/// </summary>
		/// <param name="originator"></param>
		private void Unsubscribe(IProxy originator)
		{
			originator.OnCommand -= ProxyOnCommand;
		}

		/// <summary>
		/// Called when a proxy raises a command to be sent to the remote API.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="eventArgs"></param>
		private void ProxyOnCommand(object sender, ApiClassInfoEventArgs eventArgs)
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

			foreach (IConsoleNodeBase node in DeviceBaseConsole.GetConsoleNodes(this))
				yield return node;
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
