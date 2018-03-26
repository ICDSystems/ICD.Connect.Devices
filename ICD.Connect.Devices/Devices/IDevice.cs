using ICD.Connect.API.Attributes;
using ICD.Connect.Devices.Proxies;

namespace ICD.Connect.Devices
{
	/// <summary>
	/// Interface for all hardware devices.
	/// </summary>
	[ApiClass(typeof(ProxyDevice))]
	public interface IDevice : IDeviceBase
	{
	}
}
