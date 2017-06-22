using ICD.Common.Properties;
using ICD.Connect.Devices.Controls;

namespace ICD.Connect.Devices
{
	/// <summary>
	/// Interface for all hardware devices.
	/// </summary>
	public interface IDevice : IDeviceBase
	{
		/// <summary>
		/// Gets the controls for this device.
		/// </summary>
		[PublicAPI]
		DeviceControlsCollection Controls { get; }
	}
}
