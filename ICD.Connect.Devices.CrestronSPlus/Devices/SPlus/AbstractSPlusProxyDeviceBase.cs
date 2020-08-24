using System;
using ICD.Common.Utils.Extensions;
using ICD.Connect.API;
using ICD.Connect.API.Info;
using ICD.Connect.Devices.CrestronSPlus.EventArguments;
using ICD.Connect.Devices.Proxies.Devices;

namespace ICD.Connect.Devices.CrestronSPlus.Devices.SPlus
{
	public abstract class AbstractSPlusProxyDeviceBase<TSettings> : AbstractProxyDeviceBase<TSettings>, ISPlusProxyDeviceBase
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
			CallMethod(SPlusDeviceBaseApi.METHOD_SET_IS_ONLINE, online);
		}

		/// <summary>
		/// Override to build initialization commands on top of the current class info.
		/// </summary>
		/// <param name="command"></param>
		protected override void Initialize(ApiClassInfo command)
		{
			base.Initialize(command);

			ApiCommandBuilder.UpdateCommand(command)
			                 .SubscribeEvent(SPlusDeviceBaseApi.EVENT_ON_REQUEST_SHIM_RESYNC)
			                 .Complete();
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
				case SPlusDeviceBaseApi.EVENT_ON_REQUEST_SHIM_RESYNC:
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