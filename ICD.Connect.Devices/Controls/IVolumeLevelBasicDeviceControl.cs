using System;
using System.Collections.Generic;
using System.Text;

namespace ICD.Connect.Devices.Controls
{
    public interface IVolumeLevelBasicDeviceControl : IVolumeDeviceControl
    {

		/// <summary>
		/// Raises the volume one time
		/// Ammount of the change varies between implementations - typically "1" raw unit
		/// </summary>
	    void VolumeLevelIncrement();


	    /// <summary>
	    /// Lowers the volume one time
	    /// Ammount of the change varies between implementations - typically "1" raw unit
	    /// </summary>
		void VolumeLevelDecrement();

	    /// <summary>
	    /// Starts raising the volume, and continues until RampStop is called.
	    /// <see cref="VolumeLevelRampStop"/> must be called after
	    /// </summary>
	    void VolumeLevelRampUp();

	    /// <summary>
	    /// Starts lowering the volume, and continues until RampStop is called.
	    /// <see cref="VolumeLevelRampStop"/> must be called after
	    /// </summary>
	    void VolumeLevelRampDown();

	    /// <summary>
	    /// Stops any current ramp up/down in progress.
	    /// </summary>
	    void VolumeLevelRampStop();
	}
}
