using ICD.Connect.Devices.Simpl;

namespace ICD.Connect.Devices.SPlusInterfaces
{
	public abstract class AbstractSPlusDeviceInterface<TOriginator> : AbstractSPlusDeviceBaseInterface<TOriginator>
		where TOriginator : ISimplDevice
	{
	}
}
