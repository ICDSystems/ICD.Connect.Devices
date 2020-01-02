using System;
using ICD.Common.Properties;
using ICD.Connect.Devices.Controls;
using ICD.Connect.Devices.EventArguments;
using ICD.Connect.Settings.Originators;

namespace ICD.Connect.Devices.Points
{
	public interface IPoint : IOriginator
	{
		/// <summary>
		/// Raised when the wrapped control changes.
		/// </summary>
		event EventHandler<DeviceControlEventArgs> OnControlChanged; 

		/// <summary>
		/// Gets/sets the control for this point.
		/// </summary>
		[CanBeNull]
		IDeviceControl Control { get; }

		/// <summary>
		/// Gets the device ID.
		/// </summary>
		int DeviceId { get; }

		/// <summary>
		/// Gets the control ID.
		/// </summary>
		int ControlId { get; }

		/// <summary>
		/// Sets the wrapped control.
		/// </summary>
		/// <param name="control"></param>
		void SetControl([CanBeNull] IDeviceControl control);
	}
}