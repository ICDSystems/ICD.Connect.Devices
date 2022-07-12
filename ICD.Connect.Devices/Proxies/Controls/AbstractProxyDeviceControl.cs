using System;
using System.Collections.Generic;
using ICD.Common.Logging.Activities;
using ICD.Common.Logging.LoggingContexts;
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
using ICD.Connect.Devices.Proxies.Devices;
using ICD.Connect.Devices.Utils;

namespace ICD.Connect.Devices.Proxies.Controls
{
	public abstract class AbstractProxyDeviceControl : AbstractProxy, IProxyDeviceControl
	{
		private readonly int m_Id;
		private readonly Guid m_Uuid;
		private readonly IProxyDevice m_Parent;
		private readonly ILoggingContext m_Logger;
		private readonly IActivityContext m_Activities;

		private bool m_ControlAvailable;

		#region Properties

		public event EventHandler<DeviceControlAvailableApiEventArgs> OnControlAvailableChanged;

		/// <summary>
		/// Gets the parent device for this control.
		/// </summary>
		public IDevice Parent { get { return m_Parent; } }

		/// <summary>
		/// Gets the id for this control.
		/// </summary>
		public int Id { get { return m_Id; } }

		/// <summary>
		/// Unique ID for the control.
		/// </summary>
		public Guid Uuid { get { return m_Uuid; } }

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

				Logger.LogSetTo(eSeverity.Informational, "ControlAvailable", m_ControlAvailable);

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
		public ILoggingContext Logger { get { return m_Logger; } }

		/// <summary>
		/// Gets the activities for this instance.
		/// </summary>
		public IActivityContext Activities { get { return m_Activities; } }

		#endregion

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="parent"></param>
		/// <param name="id"></param>
		protected AbstractProxyDeviceControl([NotNull] IProxyDevice parent, int id)
			: this(parent, id, DeviceControlUtils.GenerateUuid(parent, id))
		{
		}

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="parent"></param>
		/// <param name="id"></param>
		/// <param name="uuid"></param>
		protected AbstractProxyDeviceControl([NotNull] IProxyDevice parent, int id, Guid uuid)
		{
			if (parent == null)
				throw new ArgumentNullException("parent");

			m_Id = id;
			m_Uuid = uuid;
			m_Parent = parent;
			m_Logger = new ServiceLoggingContext(this);
			m_Activities = new ActivityContext();
		}

		#region Methods

		/// <summary>
		/// Gets the string representation for this instance.
		/// </summary>
		/// <returns></returns>
		public override string ToString()
		{
			return new ReprBuilder(this)
				.AppendProperty("Id", Id)
				.AppendProperty("Parent", Parent.Id)
				.ToString();
		}

		/// <summary>
		/// Initializes the current telemetry state.
		/// </summary>
		public virtual void InitializeTelemetry()
		{
		}

		#endregion

		#region Private Methods

		/// <summary>
		/// Override to build initialization commands on top of the current class info.
		/// </summary>
		/// <param name="command"></param>
		protected override void Initialize(ApiClassInfo command)
		{
			base.Initialize(command);

			ApiCommandBuilder.UpdateCommand(command)
			                 .SubscribeEvent(DeviceControlApi.EVENT_CONTROL_AVAILABLE)
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

		#endregion

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
