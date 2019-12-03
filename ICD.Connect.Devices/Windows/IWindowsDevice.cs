namespace ICD.Connect.Devices.Windows
{
	public interface IWindowsDevice : IDeviceBase
	{
		/// <summary>
		/// Gets the path to the device on windows.
		/// </summary>
		WindowsDevicePathInfo DevicePath { get; }
	}
}
