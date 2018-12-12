using ICD.Connect.Devices.Proxies.Devices;

namespace ICD.Connect.Devices.Simpl
{
	public abstract class AbstractSimplProxyDeviceBase : AbstractProxyDeviceBase, ISimplProxyDeviceBase
	{
		#region Methods
		/// <summary>
		/// Gets/sets the device online status.
		/// </summary>
		public void SetIsOnline(bool online)
		{
			CallMethod(SimplDeviceBaseApi.METHOD_SET_IS_ONLINE, online);
		}

		#endregion
	}
}