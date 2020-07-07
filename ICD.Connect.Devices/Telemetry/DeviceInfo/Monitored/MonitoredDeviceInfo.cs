using System;
using ICD.Common.Utils.EventArguments;
using ICD.Common.Utils.Extensions;
using ICD.Connect.Devices.Telemetry.DeviceInfo.Abstract;

namespace ICD.Connect.Devices.Telemetry.DeviceInfo.Monitored
{
	public sealed class MonitoredDeviceInfo : AbstractDeviceInfo<IMonitoredNetworkDeviceInfo>, IMonitoredDeviceInfo
	{
		private string m_FirmwareVersion;
		private DateTime? m_FirmwareDate;
		private DateTime? m_UptimeStart;
		private bool m_RebootSupported;

		public MonitoredDeviceInfo() : base(new MonitoredNetworkDeviceInfo())
		{
		}

		public event EventHandler<StringEventArgs> OnFirmwareVersionChanged;
		public event EventHandler<DateTimeNullableEventArgs> OnFirmwareDateChanged;
		public event EventHandler<DateTimeNullableEventArgs> OnUptimeStartChanged;
		public event EventHandler<BoolEventArgs> OnRebootSupportedChanged;

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

		public bool RebootSupported
		{
			get { return m_RebootSupported; }
			set
			{
				if (m_RebootSupported == value)
					return;

				m_RebootSupported = value;

				OnRebootSupportedChanged.Raise(this, new BoolEventArgs(value));
			}
		}

		/// <summary>
		/// Identifies the node for telemetry
		/// Should be "Monitored" or "Configured"
		/// </summary>
		public override string NodeIdentifier { get { return "Monitored"; } }

		public void Reboot()
		{
			throw new NotImplementedException();
		}
	}
}