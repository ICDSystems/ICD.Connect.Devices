namespace ICD.Connect.Devices
{
	public interface IDeviceWithPower : IDevice
	{
		/// <summary>
		/// Powers on the device.
		/// </summary>
		void PowerOn();

		/// <summary>
		/// Powers off the device.
		/// </summary>
		void PowerOff();
	}
}
