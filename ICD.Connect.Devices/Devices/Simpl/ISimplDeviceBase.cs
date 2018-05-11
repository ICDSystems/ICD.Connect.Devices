using ICD.Common.Properties;
using ICD.Connect.Settings.Originators.Simpl;

namespace ICD.Connect.Devices.Simpl
{
	public delegate bool SimplDeviceOnlineCallback(ISimplDeviceBase sender);

	public interface ISimplDeviceBase : ISimplOriginator, IDeviceBase
	{
		/// <summary>
		/// Gets/sets the online status callback.
		/// </summary>
		[PublicAPI("S+")]
		SimplDeviceOnlineCallback OnlineStatusCallback { get; set; }
	}
}
