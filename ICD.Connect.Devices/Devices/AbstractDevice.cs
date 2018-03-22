using ICD.Connect.API.Attributes;

namespace ICD.Connect.Devices
{
	/// <summary>
	/// Base class for devices.
	/// </summary>
	[ApiClass(typeof(ProxyDevice))]
	public abstract class AbstractDevice<T> : AbstractDeviceBase<T>, IDevice
		where T : IDeviceSettings, new()
	{
	}
}
