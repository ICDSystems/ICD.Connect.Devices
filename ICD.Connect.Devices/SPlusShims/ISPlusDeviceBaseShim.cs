using ICD.Common.Properties;
using ICD.Connect.Devices.Simpl;
using ICD.Connect.Settings.SPlusShims;

namespace ICD.Connect.Devices.SPlusShims
{
	public interface ISPlusDeviceBaseShim<TOriginator> : ISPlusOriginatorShim<TOriginator> 
		where TOriginator : ISimplDeviceBase
	{
		/// <summary>
		/// Returns true if the device hardware is detected by the system.
		/// </summary>
		[PublicAPI("S+")]
		ushort IsOnline { get; }
	}
}