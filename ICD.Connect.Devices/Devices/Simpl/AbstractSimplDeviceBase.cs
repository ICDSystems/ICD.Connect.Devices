using System;
using ICD.Connect.Devices.EventArguments;

namespace ICD.Connect.Devices.Simpl
{
	public abstract class AbstractSimplDeviceBase<TSettings> : AbstractDeviceBase<TSettings>, ISimplDeviceBase
		where TSettings : ISimplDeviceBaseSettings, new()
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
