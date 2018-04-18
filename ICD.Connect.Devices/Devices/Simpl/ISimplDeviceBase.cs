using ICD.Common.Properties;

namespace ICD.Connect.Devices.Simpl
{
	public delegate bool SimplDeviceOnlineCallback(ISimplDeviceBase sender);

	public interface ISimplDeviceBase : IDeviceBase
	{
		/// <summary>
		/// Gets/sets the online status callback.
		/// </summary>
		[PublicAPI("S+")]
		SimplDeviceOnlineCallback OnlineStatusCallback { get; set; }
	}
}
