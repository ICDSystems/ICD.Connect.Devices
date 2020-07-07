using System;
using ICD.Common.Utils.EventArguments;
using ICD.Common.Utils.Extensions;
using ICD.Connect.Devices.Telemetry.DeviceInfo.NetworkInfo;

namespace ICD.Connect.Devices.Telemetry.DeviceInfo
{
	public abstract class AbstractDeviceInfoTelemetry<TNetworkInfo> : IDeviceInfoTelemetry<TNetworkInfo>
		where TNetworkInfo : INetworkDeviceInfoTelemetry
	{
		private string m_Make;
		private string m_Model;
		private string m_SerialNumber;
		private readonly TNetworkInfo m_NetworkInfo;

		public virtual event EventHandler<StringEventArgs> OnMakeChanged;
		public virtual event EventHandler<StringEventArgs> OnModelChanged;
		public virtual event EventHandler<StringEventArgs> OnSerialNumberChanged;

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

		INetworkDeviceInfoTelemetry IDeviceInfoTelemetry.NetworkInfo
		{
			get { return NetworkInfo; }
		}

		/// <summary>
		/// Identifies the node for telemetry
		/// Should be "Monitored" or "Configured"
		/// </summary>
		public abstract string NodeIdentifier { get; }

		public TNetworkInfo NetworkInfo { get { return m_NetworkInfo; } }

		protected AbstractDeviceInfoTelemetry(TNetworkInfo networkInfo)
		{
			m_NetworkInfo = networkInfo;
		}
	}
}