using System;
using System.Collections.Generic;
using System.Text;

namespace ICD.Connect.Devices.Controls
{
    public interface IVolumeMuteBasicDeviceControl : IVolumeDeviceControl
    {
	    void VolumeMuteToggle();
    }
}
