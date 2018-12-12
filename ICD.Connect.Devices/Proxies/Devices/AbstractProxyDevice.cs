namespace ICD.Connect.Devices.Proxies.Devices
{
    public abstract class AbstractProxyDevice<TSettings> : AbstractProxyDeviceBase<TSettings>, IProxyDevice
		where TSettings : IProxyDeviceSettings
    {
    }
}
