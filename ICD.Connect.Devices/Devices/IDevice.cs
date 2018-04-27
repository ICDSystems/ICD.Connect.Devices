using ICD.Connect.API.Attributes;
using ICD.Connect.Devices.Proxies.Devices;

namespace ICD.Connect.Devices
{
	/// <summary>
	/// Interface for all hardware devices.
	/// </summary>
	[ApiClass(typeof(ProxyDevice), typeof(IDeviceBase))]
	public interface IDevice : IDeviceBase
	{
	}
}
