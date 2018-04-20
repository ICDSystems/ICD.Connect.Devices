namespace ICD.Connect.Devices.Simpl
{
	public abstract class AbstractSimplDevice<TSettings> : AbstractSimplDeviceBase<TSettings>, ISimplDevice
		where TSettings : ISimplDeviceSettings, new()
	{
	}
}
