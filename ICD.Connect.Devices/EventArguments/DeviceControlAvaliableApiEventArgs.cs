using ICD.Connect.API.EventArguments;
using ICD.Connect.Devices.Proxies.Controls;

namespace ICD.Connect.Devices.EventArguments
{
    public sealed class DeviceControlAvaliableApiEventArgs : AbstractGenericApiEventArgs<bool>
    {
	    /// <summary>
	    /// Constructor.
	    /// </summary>
	    /// <param name="data"></param>
	    public DeviceControlAvaliableApiEventArgs(bool data) : base(DeviceControlApi.EVENT_CONTROL_AVALIABLE, data)
	    {
	    }
    }
}
