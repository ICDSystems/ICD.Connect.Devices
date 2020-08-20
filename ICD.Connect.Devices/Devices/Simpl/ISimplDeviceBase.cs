using System;
using ICD.Common.Properties;
using ICD.Connect.API.Attributes;
using ICD.Connect.Devices.EventArguments;

namespace ICD.Connect.Devices.Simpl
{
	public interface ISimplDeviceBase : IDeviceBase
	{
		/// <summary>
		/// Gets/sets the device online status.
		/// </summary>
		[PublicAPI("S+")]
		[ApiMethod(SimplDeviceBaseApi.METHOD_SET_IS_ONLINE, SimplDeviceBaseApi.HELP_METHOD_SET_IS_ONLINE)]
		void SetIsOnline(bool online);

		[ApiEvent(SimplDeviceBaseApi.EVENT_ON_REQUEST_SHIM_RESYNC, SimplDeviceBaseApi.EVENT_ON_REQUEST_SHIM_RESYNC_HELP)]
		event EventHandler<RequestShimResyncEventArgs> OnRequestShimResync;
	}
}
