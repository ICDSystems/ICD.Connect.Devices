namespace ICD.Connect.Devices.Simpl
{
	public abstract class AbstractSimplDeviceSettings : AbstractSimplDeviceBaseSettings, ISimplDeviceSettings
	{
		/// <summary>
		/// Gets the xml element.
		/// </summary>
		protected override string Element { get { return AbstractDeviceSettings.DEVICE_ELEMENT; } }
	}
}
