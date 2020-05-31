using System;
using ICD.Common.Logging.LoggingContexts;
using ICD.Common.Utils;
using ICD.Connect.API.Attributes;
using ICD.Connect.API.Nodes;
using ICD.Connect.Devices.EventArguments;
using ICD.Connect.Devices.Proxies.Controls;
using ICD.Connect.Telemetry.Attributes;
using ICD.Connect.Telemetry.Providers;

namespace ICD.Connect.Devices.Controls
{
	/// <summary>
	/// IDeviceControl contains the usage features for a parent device.
	/// </summary>
	[ApiClass(typeof(ProxyDeviceControl))]
	[ExternalTelemetry("Device Control Telemetry", typeof(DeviceControlExternalTelemetryProvider))]
	public interface IDeviceControl : IConsoleNode, IStateDisposable, ITelemetryProvider
	{
		/// <summary>
		/// Raised when the Control availability changes
		/// </summary>
		[ApiEvent(DeviceControlApi.EVENT_CONTROL_AVAILABLE, DeviceControlApi.HELP_EVENT_CONTROL_AVAILABLE)]
		event EventHandler<DeviceControlAvailableApiEventArgs> OnControlAvailableChanged;

		/// <summary>
		/// Gets the parent device for this control.
		/// </summary>
		IDevice Parent { get; }

		/// <summary>
		/// Gets the id for this control.
		/// </summary>
		[ApiProperty(DeviceControlApi.PROPERTY_ID, DeviceControlApi.HELP_PROPERTY_ID)]
		[PropertyTelemetry(ControlTelemetryNames.ID, null, null)]
		int Id { get; }

		/// <summary>
		/// Unique ID for the control.
		/// </summary>
		[ApiProperty(DeviceControlApi.PROPERTY_UUID, DeviceControlApi.HELP_PROPERTY_UUID)]
		[PropertyTelemetry(ControlTelemetryNames.UUID, null, null)]
		[TelemetryCollectionIdentity]
		Guid Uuid { get; }

		/// <summary>
		/// Gets the human readable name for this control.
		/// </summary>
		[ApiProperty(DeviceControlApi.PROPERTY_NAME, DeviceControlApi.HELP_PROPERTY_NAME)]
		[PropertyTelemetry(ControlTelemetryNames.NAME, null, null)]
		string Name { get; }

		/// <summary>
		/// Gets if the control is currently.
		/// </summary>
		[ApiProperty(DeviceControlApi.PROPERTY_CONTROL_AVAILABLE, DeviceControlApi.HELP_PROPERTY_CONTROL_AVAILABLE)]
		bool ControlAvailable { get; }

		/// <summary>
		/// Logger for the control.
		/// </summary>
		ILoggingContext Logger { get; }
	}

	public interface IDeviceControl<T> : IDeviceControl
		where T : IDevice
	{
		/// <summary>
		/// Gets the parent device for this control.
		/// </summary>
		new T Parent { get; }
	}

	public static class DeviceControlExtensions
	{
		/// <summary>
		/// Gets the parent and control id info.
		/// </summary>
		public static DeviceControlInfo GetDeviceControlInfo(this IDeviceControl extends)
		{
			if (extends == null)
				throw new ArgumentNullException("extends");

			return new DeviceControlInfo(extends.Parent.Id, extends.Id);
		}
	}
}
