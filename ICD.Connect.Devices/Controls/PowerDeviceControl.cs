namespace ICD.Connect.Devices.Controls
{
	public sealed class PowerDeviceControl<T> : AbstractPowerDeviceControl<T>
		where T : IDeviceWithPower
	{
		public PowerDeviceControl(T parent, int id)
			: base(parent, id)
		{
		}

		protected override void PowerOnFinal()
		{
			Parent.PowerOn();
		}

		protected override void PowerOffFinal()
		{
			Parent.PowerOff();
		}
	}
}
