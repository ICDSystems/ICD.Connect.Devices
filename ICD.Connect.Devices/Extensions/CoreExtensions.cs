using System;
using System.Collections.Generic;
using System.Linq;
using ICD.Connect.Devices.Controls;
using ICD.Connect.Settings;
using ICD.Connect.Settings.Core;

namespace ICD.Connect.Devices.Extensions
{

	public sealed class CoreDeviceCollection : AbstractOriginatorCollection<IDevice>
	{
		public CoreDeviceCollection() : base()
		{
		}

		public CoreDeviceCollection(IEnumerable<IDevice> children) : base(children)
		{
		}
	}
	/// <summary>
	/// Extension methods for ICores.
	/// </summary>
	public static class CoreExtensions
	{
		public static CoreDeviceCollection GetDevices(this ICore core)
		{
			return new CoreDeviceCollection(core.Originators.OfType<IDevice>());
		} 

		/// <summary>
		/// Gets the control with the given device and control ids.
		/// </summary>
		/// <param name="extends"></param>
		/// <param name="device"></param>
		/// <param name="control"></param>
		/// <returns></returns>
		public static IDeviceControl GetControl(this ICore extends, int device, int control)
		{
			return extends.GetDevices()[device].Controls[control];
		}

		/// <summary>
		/// Gets the control with the given device and control ids.
		/// </summary>
		/// <param name="extends"></param>
		/// <param name="device"></param>
		/// <param name="control"></param>
		/// <returns></returns>
		public static T GetControl<T>(this ICore extends, int device, int control)
			where T : IDeviceControl
		{
			IDeviceControl output = extends.GetControl(device, control);
			if (output is T)
				return (T)output;

			string message = string.Format("{0} can not be cast to {1}", output.GetType().Name, typeof(T).Name);
			throw new InvalidCastException(message);
		}
	}
}