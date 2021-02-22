using System;
using ICD.Common.Utils.EventArguments;
using ICD.Common.Utils.Extensions;
using ICD.Connect.API.Nodes;
using ICD.Connect.Devices.Telemetry.DeviceInfo.Abstract;

namespace ICD.Connect.Devices.Telemetry.DeviceInfo.Monitored
{
	public sealed class MonitoredDeviceInfo : AbstractDeviceInfo<IMonitoredNetworkDeviceInfo>, IMonitoredDeviceInfo
	{
		#region Fields

		private string m_FirmwareVersion;
		private DateTime? m_FirmwareDate;
		private DateTime? m_UptimeStart;

		#endregion

		#region Events

		public event EventHandler<StringEventArgs> OnFirmwareVersionChanged;
		public event EventHandler<DateTimeNullableEventArgs> OnFirmwareDateChanged;
		public event EventHandler<DateTimeNullableEventArgs> OnUptimeStartChanged;

		#endregion

		#region Properties

		public string FirmwareVersion
		{
			get { return m_FirmwareVersion; }
			set
			{
				if (m_FirmwareVersion == value)
					return;

				m_FirmwareVersion = value;

				OnFirmwareVersionChanged.Raise(this, new StringEventArgs(value));
			}
		}

		public DateTime? FirmwareDate
		{
			get { return m_FirmwareDate; }
			set
			{
				if (m_FirmwareDate == value)
					return;

				m_FirmwareDate = value;

				OnFirmwareDateChanged.Raise(this, new DateTimeNullableEventArgs(value));
			}
		}

		public DateTime? UptimeStart
		{
			get { return m_UptimeStart; }
			set
			{
				if (m_UptimeStart == value)
					return;

				m_UptimeStart = value;

				OnUptimeStartChanged.Raise(this, new DateTimeNullableEventArgs(value));
			}
		}

		#endregion

		#region Constructor

		/// <summary>
		/// Constructor.
		/// </summary>
		public MonitoredDeviceInfo()
			: base(new MonitoredNetworkDeviceInfo())
		{
		}

		#endregion

		/// <summary>
		/// Gets the name of the node.
		/// </summary>
		public override string ConsoleName { get { return "MonitoredDeviceInfo"; } }

		/// <summary>
		/// Gets the help information for the node.
		/// </summary>
		public override string ConsoleHelp { get { return "Monitored information for the device"; } }

		/// <summary>
		/// Calls the delegate for each console status item.
		/// </summary>
		/// <param name="addRow"></param>
		public override void BuildConsoleStatus(AddStatusRowDelegate addRow)
		{
			base.BuildConsoleStatus(addRow);

			addRow("Firmware Version", FirmwareVersion);
			addRow("Firmware Date", FirmwareDate);
			addRow("Uptime Start", UptimeStart);
		}
	}
}