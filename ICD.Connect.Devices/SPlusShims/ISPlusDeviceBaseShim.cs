using ICD.Common.Properties;
using ICD.Connect.Devices.Simpl;
using ICD.Connect.Settings.SPlusShims;

namespace ICD.Connect.Devices.SPlusShims
{
	public interface ISPlusDeviceBaseShim<TOriginator> : ISPlusOriginatorShim<TOriginator> 
		where TOriginator : ISimplDeviceBase
	{
		/// <summary>
		/// Gets/sets the online status of the device.
		/// </summary>
		[PublicAPI("S+")]
		ushort IsOnline { get; set; }
	}
}