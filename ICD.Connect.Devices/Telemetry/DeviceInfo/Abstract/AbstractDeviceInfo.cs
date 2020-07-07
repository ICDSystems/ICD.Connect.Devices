using System;
using ICD.Common.Utils.EventArguments;
using ICD.Common.Utils.Extensions;

namespace ICD.Connect.Devices.Telemetry.DeviceInfo.Abstract
{
	public abstract class AbstractDeviceInfo<TNetworkInfo> : IDeviceInfo<TNetworkInfo>
		where TNetworkInfo : INetworkDeviceInfo
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

		INetworkDeviceInfo IDeviceInfo.NetworkInfo
		{
			get { return NetworkInfo; }
		}

		public TNetworkInfo NetworkInfo { get { return m_NetworkInfo; } }

		protected AbstractDeviceInfo(TNetworkInfo networkInfo)
		{
			m_NetworkInfo = networkInfo;
		}

		/// <summary>
		/// Initializes the current telemetry state.
		/// </summary>
		public void InitializeTelemetry()
		{
		}
	}
}