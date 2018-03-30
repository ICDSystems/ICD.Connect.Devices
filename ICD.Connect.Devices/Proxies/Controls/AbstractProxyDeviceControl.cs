using System.Collections.Generic;
using ICD.Common.Utils;
using ICD.Common.Utils.Services.Logging;
using ICD.Connect.API.Commands;
using ICD.Connect.API.Nodes;
using ICD.Connect.API.Proxies;
using ICD.Connect.Devices.Controls;
using ICD.Connect.Devices.Proxies.Devices;

namespace ICD.Connect.Devices.Proxies.Controls
{
	public abstract class AbstractProxyDeviceControl : AbstractProxy, IProxyDeviceControl
	{
		private readonly int m_Id;
		private readonly IProxyDeviceBase m_Parent;

		#region Properties

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
		public virtual string Name { get { return GetType().Name; } }

		/// <summary>
		/// Gets the parent and control id info.
		/// </summary>
		public DeviceControlInfo DeviceControlInfo { get { return new DeviceControlInfo(Parent.Id, Id); } }

		/// <summary>
		/// Gets the name of the node.
		/// </summary>
		public virtual string ConsoleName { get { return string.IsNullOrEmpty(Name) ? GetType().Name : Name; } }

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
