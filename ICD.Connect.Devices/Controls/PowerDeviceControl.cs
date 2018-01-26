using System;
using System.Collections.Generic;
using System.Text;

namespace ICD.Connect.Devices.Controls
{
    public class PowerDeviceControl<T> : AbstractPowerDeviceControl<T>
		where T : IDeviceWithPower
    {
	    public PowerDeviceControl(T parent, int id) : base(parent, id)
	    {
	    }

	    public override void PowerOn()
	    {
		    Parent.PowerOn();
	    }

	    public override void PowerOff()
	    {
		    Parent.PowerOff();
	    }
    }
}
