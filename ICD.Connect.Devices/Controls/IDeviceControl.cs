using System;
using System.Collections.Generic;
using System.Linq;
using ICD.Common.Logging.LoggingContexts;
using ICD.Common.Properties;
using ICD.Common.Utils;
using ICD.Common.Utils.Extensions;
using ICD.Connect.API.Attributes;
using ICD.Connect.API.Nodes;
using ICD.Connect.Devices.EventArguments;
using ICD.Connect.Devices.Proxies.Controls;
using ICD.Connect.Telemetry;
using ICD.Connect.Telemetry.Attributes;

namespace ICD.Connect.Devices.Controls
{
	/// <summary>
	/// IDeviceControl contains the usage features for a parent device.
	/// </summary>
	[ApiClass(typeof(ProxyDeviceControl))]
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
		IDeviceBase Parent { get; }

		/// <summary>
		/// Gets the id for this control.
		/// </summary>
		[ApiProperty(DeviceControlApi.PROPERTY_ID, DeviceControlApi.HELP_PROPERTY_ID)]
		[StaticPropertyTelemetry(ControlTelemetryNames.ID)]
		int Id { get; }

		/// <summary>
		/// Gets the human readable name for this control.
		/// </summary>
		[ApiProperty(DeviceControlApi.PROPERTY_NAME, DeviceControlApi.HELP_PROPERTY_NAME)]
		[StaticPropertyTelemetry(ControlTelemetryNames.NAME)]
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
		where T : IDeviceBase
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

		/// <summary>
		/// Gets the sibling control with the given id.
		/// </summary>
		/// <param name="extends"></param>
		/// <param name="id"></param>
		/// <returns></returns>
		[NotNull]
		[PublicAPI]
		public static IDeviceControl GetSibling(this IDeviceControl extends, int id)
		{
			if (extends == null)
				throw new ArgumentNullException("extends");

			return extends.Parent
			              .Controls
			              .GetControl(id);
		}

		/// <summary>
		/// Gets the first sibling control of the given type.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="extends"></param>
		/// <returns></returns>
		[NotNull]
		[PublicAPI]
		public static T GetSibling<T>(this IDeviceControl extends)
			where T : IDeviceControl
		{
			if (extends == null)
				throw new ArgumentNullException("extends");

			return extends.GetSiblings()
			              .Except(extends)
			              .OfType<T>()
			              .First();
		}

		/// <summary>
		/// Gets the sibling control with the given id and type.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="extends"></param>
		/// <param name="id"></param>
		/// <returns></returns>
		[NotNull]
		[PublicAPI]
		public static T GetSibling<T>(this IDeviceControl extends, int id)
			where T : class, IDeviceControl
		{
			if (extends == null)
				throw new ArgumentNullException("extends");

			return extends.Parent
			              .Controls
			              .GetControl<T>(id);
		}

		/// <summary>
		/// Gets all of the controls on the parent device except this control.
		/// </summary>
		/// <param name="extends"></param>
		/// <returns></returns>
		[NotNull]
		[PublicAPI]
		public static IEnumerable<IDeviceControl> GetSiblings(this IDeviceControl extends)
		{
			if (extends == null)
				throw new ArgumentNullException("extends");

			return extends.Parent
			              .Controls
			              .Except(extends);
		}

		/// <summary>
		/// Gets all of the controls of the given type on the parent device except this control.
		/// </summary>
		/// <param name="extends"></param>
		/// <returns></returns>
		[NotNull]
		[PublicAPI]
		public static IEnumerable<T> GetSiblings<T>(this IDeviceControl extends)
		{
			if (extends == null)
				throw new ArgumentNullException("extends");

			return extends.GetSiblings()
			              .OfType<T>();
		}
	}
}
