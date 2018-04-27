namespace ICD.Connect.Devices.Simpl
{
	public abstract class AbstractSimplDeviceBase<TSettings> : AbstractDeviceBase<TSettings>, ISimplDeviceBase
		where TSettings : ISimplDeviceBaseSettings, new()
	{
		/// <summary>
		/// Gets/sets the online status callback.
		/// </summary>
		public SimplDeviceOnlineCallback OnlineStatusCallback { get; set; }

		/// <summary>
		/// Release resources.
		/// </summary>
		protected override void DisposeFinal(bool disposing)
		{
			OnlineStatusCallback = null;

			base.DisposeFinal(disposing);
		}

		/// <summary>
		/// Gets the current online status of the device.
		/// </summary>
		/// <returns></returns>
		protected override bool GetIsOnlineStatus()
		{
			SimplDeviceOnlineCallback handler = OnlineStatusCallback;
			return handler != null && handler(this);
		}
	}
}
