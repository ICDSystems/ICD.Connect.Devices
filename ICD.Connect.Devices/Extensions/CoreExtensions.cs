using System;
using System.Collections.Generic;
using System.Linq;
using ICD.Common.Properties;
using ICD.Connect.Devices.Controls;
using ICD.Connect.Settings;
using ICD.Connect.Settings.Core;

namespace ICD.Connect.Devices.Extensions
{
	/// <summary>
	/// Extension methods for ICores.
	/// </summary>
	public static class CoreExtensions
	{
		/// <summary>
		/// Gets the control with the given device and control ids.
		/// </summary>
		/// <param name="extends"></param>
		/// <param name="controlInfo"></param>
		/// <returns></returns>
		[NotNull]
		public static IDeviceControl GetControl(this ICore extends, DeviceControlInfo controlInfo)
		{
			if (extends == null)
				throw new ArgumentNullException("extends");

			return extends.GetControl(controlInfo.DeviceId, controlInfo.ControlId);
		}

		/// <summary>
		/// Gets the control with the given device and control ids.
		/// </summary>
		/// <param name="extends"></param>
		/// <param name="deviceId"></param>
		/// <param name="controlId"></param>
		/// <returns></returns>
		[NotNull]
		public static IDeviceControl GetControl(this ICore extends, int deviceId, int controlId)
		{
			if (extends == null)
				throw new ArgumentNullException("extends");

			IOriginator originator;
			if (!extends.Originators.TryGetChild(deviceId, out originator))
				throw new KeyNotFoundException(string.Format("No device with id {0}", deviceId));

			IDeviceBase device = originator as IDeviceBase;
			if (device == null)
				throw new KeyNotFoundException(string.Format("{0} is not of type {1}", originator, typeof(IDeviceBase).Name));

			IDeviceControl control;
			if (!device.Controls.TryGetControl(controlId, out control))
				throw new KeyNotFoundException(string.Format("{0} does not contain a control with id {1}", originator, controlId));

			return control;
		}

		/// <summary>
		/// Gets the control with the given device and control ids.
		/// </summary>
		/// <param name="extends"></param>
		/// <param name="controlInfo"></param>
		/// <returns></returns>
		[NotNull]
		public static T GetControl<T>(this ICore extends, DeviceControlInfo controlInfo)
			where T : IDeviceControl
		{
			if (extends == null)
				throw new ArgumentNullException("extends");

			return extends.GetControl<T>(controlInfo.DeviceId, controlInfo.ControlId);
		}

		/// <summary>
		/// Gets the control with the given device and control ids.
		/// </summary>
		/// <param name="extends"></param>
		/// <param name="device"></param>
		/// <param name="control"></param>
		/// <returns></returns>
		[NotNull]
		public static T GetControl<T>(this ICore extends, int device, int control)
			where T : IDeviceControl
		{
			if (extends == null)
				throw new ArgumentNullException("extends");

			IDeviceControl output = extends.GetControl(device, control);
			if (output is T)
				return (T)output;

			string message = string.Format("{0} can not be cast to {1}", output, typeof(T).Name);
			throw new InvalidCastException(message);
		}

		/// <summary>
		/// Gets the control with the given device and control ids.
		/// </summary>
		/// <param name="extends"></param>
		/// <param name="controlInfo"></param>
		/// <returns></returns>
		[NotNull]
		public static IEnumerable<T> GetControls<T>(this ICore extends, IEnumerable<DeviceControlInfo> controlInfo)
			where T : IDeviceControl
		{
			if (extends == null)
				throw new ArgumentNullException("extends");

			if (extends == null)
				throw new ArgumentNullException("controlInfo");

			DeviceControlInfo[] info = controlInfo as DeviceControlInfo[] ?? controlInfo.ToArray();

			Dictionary<int, IDeviceBase> devices =
				extends.Originators
				       .GetChildren<IDeviceBase>(info.Select(i => i.DeviceId)
				                                     .Distinct())
				       .ToDictionary(d => d.Id);

			return info.Select(i => devices[i.DeviceId].Controls.GetControl<T>(i.ControlId));
		}
	}
}
