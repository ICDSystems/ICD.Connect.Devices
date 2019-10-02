using System;
using System.Collections.Generic;
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
using ICD.Connect.Devices.Proxies.Devices;

namespace ICD.Connect.Devices.Proxies.Controls
{
	public abstract class AbstractProxyDeviceControl : AbstractProxy, IProxyDeviceControl
	{
		public event EventHandler OnRequestTelemetryRebuild;

		private readonly int m_Id;
		private readonly IProxyDeviceBase m_Parent;
		private bool m_ControlAvailable;

		#region Properties

		public event EventHandler<DeviceControlAvailableApiEventArgs> OnControlAvailableChanged;

		/// <summary>
		/// Gets the parent device for this control.
		/// </summary>
		public IDeviceBase Parent { get { return m_Parent; } }

		/// <summary>
		/// Gets the id for this control.
		/// </summary>
		public int Id { get { return m_Id; } }

		/// <summary>
		/// Gets the human readable name for this control.
		/// </summary>
		public string Name { get; set; }

		/// <summary>
		/// Gets if the control is currently available or not
		/// </summary>
		public bool ControlAvailable
		{
			get { return m_ControlAvailable; }
			private set
			{
				if (value == m_ControlAvailable)
					return;

				m_ControlAvailable = value;

				OnControlAvailableChanged.Raise(this, new DeviceControlAvailableApiEventArgs(ControlAvailable));
			}
		}

		/// <summary>
		/// Gets the name of the node.
		/// </summary>
		public virtual string ConsoleName { get { return string.IsNullOrEmpty(Name) ? GetType().GetNameWithoutGenericArity() : Name; } }

		/// <summary>
		/// Gets the help information for the node.
		/// </summary>
		public virtual string ConsoleHelp { get { return string.Empty; } }

		/// <summary>
		/// Gets the logger for the control.
		/// </summary>
		public ILoggerService Logger { get { return Parent.Logger; } }

		#endregion

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="parent"></param>
		/// <param name="id"></param>
		protected AbstractProxyDeviceControl(IProxyDeviceBase parent, int id)
		{
			m_Id = id;
			m_Parent = parent;
		}

		#region Methods

		public void Log(eSeverity severity, string message)
		{
			Logger.AddEntry(severity, "{0} - {1}", this, message);
		}

		public void Log(eSeverity severity, string message, params object[] args)
		{
			message = string.Format(message, args);
			Log(severity, message);
		}

		/// <summary>
		/// Gets the string representation for this instance.
		/// </summary>
		/// <returns></returns>
		public override string ToString()
		{
			ReprBuilder builder = new ReprBuilder(this);

			builder.AppendProperty("Id", Id);
			builder.AppendProperty("Parent", Parent.Id);

			return builder.ToString();
		}

		#endregion

		/// <summary>
		/// Override to build initialization commands on top of the current class info.
		/// </summary>
		/// <param name="command"></param>
		protected override void Initialize(ApiClassInfo command)
		{
			base.Initialize(command);

			ApiCommandBuilder.UpdateCommand(command)
			                 .GetProperty(DeviceControlApi.PROPERTY_NAME)
			                 .GetProperty(DeviceControlApi.PROPERTY_CONTROL_AVAILABLE)
			                 .Complete();
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
				case DeviceControlApi.PROPERTY_NAME:
					Name = result.GetValue<string>();
					break;
				case DeviceControlApi.PROPERTY_CONTROL_AVAILABLE:
					ControlAvailable = result.GetValue<bool>();
					break;
			}
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
				case DeviceControlApi.EVENT_CONTROL_AVAILABLE:
					ControlAvailable = result.GetValue<bool>();
					break;
			}
		}

		#region Console

		/// <summary>
		/// Gets the child console nodes.
		/// </summary>
		/// <returns></returns>
		public virtual IEnumerable<IConsoleNodeBase> GetConsoleNodes()
		{
			return DeviceControlConsole.GetConsoleNodes(this);
		}

		/// <summary>
		/// Calls the delegate for each console status item.
		/// </summary>
		/// <param name="addRow"></param>
		public virtual void BuildConsoleStatus(AddStatusRowDelegate addRow)
		{
			DeviceControlConsole.BuildConsoleStatus(this, addRow);
		}

		/// <summary>
		/// Gets the child console commands.
		/// </summary>
		/// <returns></returns>
		public virtual IEnumerable<IConsoleCommand> GetConsoleCommands()
		{
			return DeviceControlConsole.GetConsoleCommands(this);
		}

		#endregion
	}
}
