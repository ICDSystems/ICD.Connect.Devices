using System;
using ICD.Common.Utils;
using ICD.Connect.API.Attributes;
using ICD.Connect.Devices.Controls;
using ICD.Connect.Devices.EventArguments;
using ICD.Connect.Devices.Proxies.Devices;
using ICD.Connect.Settings.Originators;
using ICD.Connect.Telemetry.Attributes;

namespace ICD.Connect.Devices
{
	[ApiClass(typeof(ProxyDeviceBase), typeof(IOriginator))]
	public interface IDeviceBase : IOriginator, IStateDisposable
	{
		/// <summary>
		/// Raised when the device goes online/offline.
		/// </summary>
		[ApiEvent(DeviceBaseApi.EVENT_IS_ONLINE, DeviceBaseApi.HELP_EVENT_IS_ONLINE)]
		[EventTelemetry(DeviceTelemetryNames.ONLINE_STATE_CHANGED)]
		event EventHandler<DeviceBaseOnlineStateApiEventArgs> OnIsOnlineStateChanged;

		/// <summary>
		/// Raised when control availability for the device changes.
		/// </summary>
		[ApiEvent(DeviceBaseApi.EVENT_CONTROLS_AVAILABLE, DeviceBaseApi.HELP_EVENT_CONTROLS_AVAILABLE)]
		event EventHandler<DeviceBaseControlsAvailableApiEventArgs> OnControlsAvailableChanged;

		/// <summary>
		/// Returns true if the device hardware is detected by the system.
		/// </summary>
		[ApiProperty(DeviceBaseApi.PROPERTY_IS_ONLINE, DeviceBaseApi.HELP_PROPERTY_IS_ONLINE)]
		[DynamicPropertyTelemetry(DeviceTelemetryNames.ONLINE_STATE, null, DeviceTelemetryNames.ONLINE_STATE_CHANGED)]
		bool IsOnline { get; }

		/// <summary>
		/// Gets the controls for this device.
		/// </summary>
		[ApiNodeGroup(DeviceBaseApi.NODE_GROUP_CONTROLS, DeviceBaseApi.HELP_NODE_GROUP_CONTROLS)]
		[CollectionTelemetry("Controls")]
		DeviceControlsCollection Controls { get; }

		/// <summary>
		/// Gets if controls are available.
		/// </summary>
		[ApiProperty(DeviceBaseApi.PROPERTY_CONTROLS_AVAILABLE, DeviceBaseApi.HELP_PROPERTY_CONTROLS_AVAILABLE)]
		bool ControlsAvailable { get; }
	}
}
