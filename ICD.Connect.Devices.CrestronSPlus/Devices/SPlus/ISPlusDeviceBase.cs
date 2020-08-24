using System;
using ICD.Common.Properties;
using ICD.Connect.API.Attributes;
using ICD.Connect.Devices.CrestronSPlus.EventArguments;

namespace ICD.Connect.Devices.CrestronSPlus.Devices.SPlus
{
	public interface ISPlusDeviceBase : IDeviceBase
	{
		/// <summary>
		/// Gets/sets the device online status.
		/// </summary>
		[PublicAPI("S+")]
		[ApiMethod(SPlusDeviceBaseApi.METHOD_SET_IS_ONLINE, SPlusDeviceBaseApi.HELP_METHOD_SET_IS_ONLINE)]
		void SetIsOnline(bool online);

		[ApiEvent(SPlusDeviceBaseApi.EVENT_ON_REQUEST_SHIM_RESYNC, SPlusDeviceBaseApi.EVENT_ON_REQUEST_SHIM_RESYNC_HELP)]
		event EventHandler<RequestShimResyncEventArgs> OnRequestShimResync;
	}
}
