using ICD.Connect.Devices.Simpl;

namespace ICD.Connect.Devices.SPlusShims
{
	public abstract class AbstractSPlusDeviceShim<TOriginator> : AbstractSPlusDeviceBaseShim<TOriginator>
		where TOriginator : class, ISimplDevice
	{
	}
}
