using System;
using ICD.Connect.Devices.Controls.Power;
using ICD.Connect.Devices.EventArguments;

namespace ICD.Connect.Devices
{
	public interface IDeviceWithPower : IDevice
	{
		/// <summary>
		/// Raised when the powered state changes.
		/// </summary>
		event EventHandler<PowerDeviceControlPowerStateApiEventArgs> OnPowerStateChanged;

		/// <summary>
		/// Gets the powered state of the device.
		/// </summary>
		ePowerState PowerState { get; }

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
