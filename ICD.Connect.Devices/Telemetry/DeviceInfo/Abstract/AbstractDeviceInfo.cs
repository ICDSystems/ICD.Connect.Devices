using System;
using System.Collections.Generic;
using ICD.Common.Utils.EventArguments;
using ICD.Common.Utils.Extensions;
using ICD.Connect.API.Commands;
using ICD.Connect.API.Nodes;

namespace ICD.Connect.Devices.Telemetry.DeviceInfo.Abstract
{
	public abstract class AbstractDeviceInfo<TNetworkInfo> : IDeviceInfo<TNetworkInfo>
		where TNetworkInfo : INetworkDeviceInfo
	{
		#region Fields

		private string m_Make;
		private string m_Model;
		private string m_SerialNumber;
		private readonly TNetworkInfo m_NetworkInfo;

		#endregion

		#region Events

		public virtual event EventHandler<StringEventArgs> OnMakeChanged;
		public virtual event EventHandler<StringEventArgs> OnModelChanged;
		public virtual event EventHandler<StringEventArgs> OnSerialNumberChanged;

		#endregion

		#region Properties

		public string Make
		{
			get { return m_Make; }
			set
			{
				if (value == m_Make)
					return;

				m_Make = value;

				OnMakeChanged.Raise(this, new StringEventArgs(value));
			}
		}

		public string Model
		{
			get { return m_Model; }
			set
			{
				if (value == m_Model)
					return;

				m_Model = value;

				OnModelChanged.Raise(this, new StringEventArgs(value));
			}
		}

		public string SerialNumber
		{
			get { return m_SerialNumber; }
			set
			{
				if (value == m_SerialNumber)
					return;

				m_SerialNumber = value;

				OnSerialNumberChanged.Raise(this, new StringEventArgs(value));
			}
		}

		INetworkDeviceInfo IDeviceInfo.NetworkInfo
		{
			get { return NetworkInfo; }
		}

		public TNetworkInfo NetworkInfo { get { return m_NetworkInfo; } }

		#endregion

		#region Constructor

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="networkInfo"></param>
		protected AbstractDeviceInfo(TNetworkInfo networkInfo)
		{
			m_NetworkInfo = networkInfo;
		}

		#endregion

		#region Methods

		/// <summary>
		/// Initializes the current telemetry state.
		/// </summary>
		public void InitializeTelemetry()
		{
		}

		#endregion

		#region Console

		/// <summary>
		/// Gets the name of the node.
		/// </summary>
		public virtual string ConsoleName { get { return "DeviceInfo"; } }

		/// <summary>
		/// Gets the help information for the node.
		/// </summary>
		public virtual string ConsoleHelp { get { return "Device information status"; } }

		/// <summary>
		/// Gets the child console nodes.
		/// </summary>
		/// <returns></returns>
		public virtual IEnumerable<IConsoleNodeBase> GetConsoleNodes()
		{
			yield break;
		}

		/// <summary>
		/// Calls the delegate for each console status item.
		/// </summary>
		/// <param name="addRow"></param>
		public virtual void BuildConsoleStatus(AddStatusRowDelegate addRow)
		{
			addRow("Make", Make);
			addRow("Model", Model);
			addRow("Serial Number", SerialNumber);
			addRow("Hostname", NetworkInfo.Hostname);
			addRow("Dns", NetworkInfo.Dns);
		}

		/// <summary>
		/// Gets the child console commands.
		/// </summary>
		/// <returns></returns>
		public virtual IEnumerable<IConsoleCommand> GetConsoleCommands()
		{
			yield break;
		}

		#endregion
	}
}