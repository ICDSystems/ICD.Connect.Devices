using ICD.Common.Properties;
using ICD.Connect.Settings.Originators.Simpl;

namespace ICD.Connect.Devices.Simpl
{
	public interface ISimplDeviceBase : ISimplOriginator, IDeviceBase
	{
		/// <summary>
		/// Gets/sets the device online status.
		/// </summary>
		[PublicAPI("S+")]
		new bool IsOnline { get; set; }
	}
}
