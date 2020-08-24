using ICD.Connect.Devices.CrestronSPlus.Devices.SPlus;

namespace ICD.Connect.Devices.CrestronSPlus.SPlusShims
{
	public abstract class AbstractSPlusDeviceShim<TOriginator> : AbstractSPlusDeviceBaseShim<TOriginator>
		where TOriginator : class, ISPlusDevice
	{
	}
}
