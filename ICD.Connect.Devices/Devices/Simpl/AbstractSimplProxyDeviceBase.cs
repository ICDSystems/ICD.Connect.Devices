using System;
using ICD.Common.Utils.Extensions;
using ICD.Connect.API.Info;
using ICD.Connect.Devices.Proxies.Devices;
using ICD.Connect.Settings.Simpl;

namespace ICD.Connect.Devices.Simpl
{
	public abstract class AbstractSimplProxyDeviceBase<TSettings> : AbstractProxyDeviceBase<TSettings>, ISimplProxyDeviceBase
		where TSettings : IProxyDeviceBaseSettings
	{

		#region Events

		/// <summary>
		/// Event to request any subscribed shims re-sync with the originator
		/// </summary>
		public event EventHandler<RequestShimResyncEventArgs> OnRequestShimResync;

		#endregion

		#region Methods
		
		/// <summary>
		/// Gets/sets the device online status.
		/// </summary>
		public void SetIsOnline(bool online)
		{
			CallMethod(SimplDeviceBaseApi.METHOD_SET_IS_ONLINE, online);
		}

		/// <summary>
		/// Updates the proxy with event feedback info.
		/// </summary>
		/// <param name="name"></param>
		/// <param name="result"></param>
		protected override void ParseEvent(string name, ApiResult result)
		{
			base.ParseEvent(name, result);
			switch (name)
			{
				case SimplOriginatorApi.EVENT_ON_REQUEST_SHIM_RESYNC:
					RaiseOnRequestShimResync(this);
					break;
			}
		}

		protected void RaiseOnRequestShimResync(object sender)
		{
			OnRequestShimResync.Raise(sender, new RequestShimResyncEventArgs());
		}

		#endregion
	}
}