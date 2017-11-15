using ICD.Connect.Devices;

namespace ICD.Connect.Settings
{
	public abstract class AbstractDeviceBaseSettings : AbstractSettings, IDeviceBaseSettings
	{
		/// <summary>
		/// Parses the xml and applies the properties to the instance.
		/// </summary>
		/// <param name="instance"></param>
		/// <param name="xml"></param>
		protected static void ParseXml(AbstractDeviceBaseSettings instance, string xml)
		{
			AbstractSettings.ParseXml(instance, xml);
		}
	}
}
