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

		public MonitoredDeviceInfo() : base(new MonitoredNetworkDeviceInfo())
		{
		}

		public event EventHandler<StringEventArgs> OnFirmwareVersionChanged;
		public event EventHandler<DateTimeNullableEventArgs> OnFirmwareDateChanged;
		public event EventHandler<DateTimeNullableEventArgs> OnUptimeStartChanged;

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
	}
}