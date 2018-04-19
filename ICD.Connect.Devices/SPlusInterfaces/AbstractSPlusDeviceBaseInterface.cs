using ICD.Connect.Devices.Simpl;
using ICD.Connect.Settings.SPlusInterfaces;

namespace ICD.Connect.Devices.SPlusInterfaces
{
	public abstract class AbstractSPlusDeviceBaseInterface<TOriginator> : AbstractSPlusOriginatorInterface<TOriginator>
		where TOriginator : ISimplDeviceBase
	{
	}
}
