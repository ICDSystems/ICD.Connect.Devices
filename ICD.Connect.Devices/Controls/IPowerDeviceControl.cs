using System;
using ICD.Common.Utils.EventArguments;

namespace ICD.Connect.Devices.Controls
{
	public interface IPowerDeviceControl : IDeviceControl
	{
		/// <summary>
		/// Raised when the powered state changes.
		/// </summary>
		event EventHandler<BoolEventArgs> OnIsPoweredChanged;

		/// <summary>
		/// Gets the powered state of the device.
		/// </summary>
		bool IsPowered { get; }

		/// <summary>
		/// Powers on the device.
		/// </summary>
		void PowerOn();

		/// <summary>
		/// Powers off the device.
		/// </summary>
		void PowerOff();
	}
}
