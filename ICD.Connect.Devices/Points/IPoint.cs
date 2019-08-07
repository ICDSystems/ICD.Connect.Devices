using System;
using ICD.Common.Properties;
using ICD.Common.Utils.Services;
using ICD.Connect.Devices.Controls;
using ICD.Connect.Settings.Cores;
using ICD.Connect.Settings.Originators;

namespace ICD.Connect.Devices.Points
{
	public interface IPoint : IOriginator
	{
		/// <summary>
		/// Device id
		/// </summary>
		int DeviceId { get; set; }

		/// <summary>
		/// Control id.
		/// </summary>
		int ControlId { get; set; }
	}

	public static class PointExtensions
	{
		/// <summary>
		/// Gets the control that this point references.
		/// </summary>
		/// <param name="extends"></param>
		/// <returns></returns>
		[NotNull]
		public static IDeviceControl GetControl([NotNull] this IPoint extends)
		{
			if (extends == null)
				throw new ArgumentNullException("extends");

			return extends.GetControl<IDeviceControl>();
		}

		/// <summary>
		/// Gets the control that this point references.
		/// </summary>
		/// <param name="extends"></param>
		/// <returns></returns>
		[NotNull]
		public static TControl GetControl<TControl>([NotNull] this IPoint extends)
			where TControl : class, IDeviceControl
		{
			if (extends == null)
				throw new ArgumentNullException("extends");

			return
				ServiceProvider.GetService<ICore>()
				               .Originators
				               .GetChild<IDeviceBase>(extends.DeviceId)
				               .Controls
				               .GetControl<TControl>(extends.ControlId);
		}

		/// <summary>
		/// Trys to get the control that this point references.
		/// </summary>
		/// <param name="extends"></param>
		/// <param name="output"></param>
		/// <returns></returns>
		public static bool TryGetControl<TControl>([NotNull] this IPoint extends, out TControl output)
			where TControl : class, IDeviceControl
		{
			if (extends == null)
				throw new ArgumentNullException("extends");

			output = null;

			IOriginator originator;
			if (!ServiceProvider.GetService<ICore>().Originators.TryGetChild(extends.DeviceId, out originator))
				return false;

			IDeviceBase device = originator as IDeviceBase;
			if (device == null)
				return false;

			IDeviceControl control;
			if (!device.Controls.TryGetControl(extends.ControlId, out control))
				return false;

			output = control as TControl;
			return output != null;
		}
	}
}