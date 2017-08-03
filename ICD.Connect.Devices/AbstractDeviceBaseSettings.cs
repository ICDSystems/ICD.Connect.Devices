using System.Collections.Generic;

namespace ICD.Connect.Settings
{
	public abstract class AbstractDeviceBaseSettings : AbstractSettings, IDeviceBaseSettings
	{
		/// <summary>
		/// Returns the collection of ids that the settings will depend on.
		/// For example, to instantiate an IR Port from settings, the device the physical port
		/// belongs to will need to be instantiated first.
		/// </summary>
		/// <returns></returns>
		public override IEnumerable<int> GetDeviceDependencies()
		{
			// Typically a device doesn't depend directly on other devices.
			yield break;
		}

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
