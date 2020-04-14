﻿using ICD.Common.Utils.Extensions;
using ICD.Connect.Telemetry;
using ICD.Connect.Telemetry.Attributes;

namespace ICD.Connect.Devices.Controls
{
	public sealed class DeviceControlExternalTelemetryProvider : IDeviceControlExternalTelemetryProvider
	{
		private IDeviceControl m_Parent;

		/// <summary>
		/// Sets the parent telemetry provider that this instance extends.
		/// </summary>
		/// <param name="provider"></param>
		public void SetParent(ITelemetryProvider provider)
		{
			m_Parent = provider as IDeviceControl;
		}

		public string DeviceControlType { get { return m_Parent.GetType().GetMinimalName(); } }
	}

	public interface IDeviceControlExternalTelemetryProvider : IExternalTelemetryProvider
	{
		[StaticPropertyTelemetry("DeviceControlType")]
		string DeviceControlType { get; }
	}
}