using ICD.Connect.Devices.Proxies.Devices;

namespace ICD.Connect.Devices.Simpl
{
	public abstract class AbstractSimplProxyDevice<TSettings> : AbstractSimplProxyDeviceBase<TSettings>, ISimplProxyDevice
		where TSettings : IProxyDeviceBaseSettings
	{
	}
}