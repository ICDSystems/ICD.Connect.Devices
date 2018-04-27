namespace ICD.Connect.Devices
{
	public interface IDeviceWithPower : IDevice
	{
		void PowerOn();

		void PowerOff();
	}
}
