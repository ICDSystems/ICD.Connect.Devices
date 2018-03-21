namespace ICD.Connect.Devices.Controls
{
	public interface IDeviceWithPower : IDevice
	{
		void PowerOn();

		void PowerOff();
	}
}
