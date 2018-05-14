namespace ICD.Connect.Devices
{
	public interface IDeviceWithPower : IDevice
	{
		/// <summary>
		/// Powers the device on.
		/// </summary>
		void PowerOn();

		/// <summary>
		/// Powers the device off.
		/// </summary>
		void PowerOff();
	}
}
