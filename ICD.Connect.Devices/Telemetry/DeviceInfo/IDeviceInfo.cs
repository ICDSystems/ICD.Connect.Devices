﻿using System;
using ICD.Common.Properties;
using ICD.Common.Utils.EventArguments;
using ICD.Connect.API.Nodes;
using ICD.Connect.Telemetry.Attributes;
using ICD.Connect.Telemetry.Providers;

namespace ICD.Connect.Devices.Telemetry.DeviceInfo
{
	/// <summary>
	/// Interface to hold collection of configured and monitored device info for telemetry
	/// </summary>
	public interface IDeviceInfo: ITelemetryProvider, IConsoleNode
	{
		[EventTelemetry(DeviceTelemetryNames.DEVICE_MAKE_CHANGED)]
		event EventHandler<StringEventArgs> OnMakeChanged;

		[EventTelemetry(DeviceTelemetryNames.DEVICE_MODEL_CHANGED)]
		event EventHandler<StringEventArgs> OnModelChanged;

		[EventTelemetry(DeviceTelemetryNames.DEVICE_SERIAL_NUMBER_CHANGED)]
		event EventHandler<StringEventArgs> OnSerialNumberChanged;

		[CanBeNull]
		[PropertyTelemetry(DeviceTelemetryNames.DEVICE_MAKE, null, DeviceTelemetryNames.DEVICE_MAKE_CHANGED)]
		string Make { get; set; }
		
		[CanBeNull]
		[PropertyTelemetry(DeviceTelemetryNames.DEVICE_MODEL, null, DeviceTelemetryNames.DEVICE_MODEL_CHANGED)]
		string Model { get; set; }

		[CanBeNull]
		[PropertyTelemetry(DeviceTelemetryNames.DEVICE_SERIAL_NUMBER, null, DeviceTelemetryNames.DEVICE_SERIAL_NUMBER_CHANGED)]
		string SerialNumber { get; set; }

		[NotNull]
		[NodeTelemetry("NetworkInfo")]
		INetworkDeviceInfo NetworkInfo { get; }
	}

	public interface IDeviceInfo<TNetworkInfo> : IDeviceInfo
		where TNetworkInfo : INetworkDeviceInfo
	{
		[NotNull]
		new TNetworkInfo NetworkInfo { get; }
	}
}