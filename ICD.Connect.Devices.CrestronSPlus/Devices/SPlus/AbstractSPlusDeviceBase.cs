using System;
using ICD.Connect.Devices.CrestronSPlus.EventArguments;

namespace ICD.Connect.Devices.CrestronSPlus.Devices.SPlus
{
	public abstract class AbstractSPlusDeviceBase<TSettings> : AbstractDeviceBase<TSettings>, ISPlusDeviceBase
		where TSettings : ISPlusDeviceBaseSettings, new()
	{

		public event EventHandler<RequestShimResyncEventArgs> OnRequestShimResync;

		private bool m_IsOnline;

		#region Methods

		public void SetIsOnline(bool online)
		{
			m_IsOnline = online;
			UpdateCachedOnlineStatus();
		}

		/// <summary>
		/// Gets the current online status of the device.
		/// </summary>
		/// <returns></returns>
		protected override bool GetIsOnlineStatus()
		{
			return m_IsOnline;
		}

		#endregion
	}
}
