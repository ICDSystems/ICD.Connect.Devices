using System;
using ICD.Common.Properties;
using ICD.Common.Utils.EventArguments;
using ICD.Common.Utils.Extensions;
using ICD.Connect.Devices.EventArguments;
using ICD.Connect.Devices.Simpl;
using ICD.Connect.Settings.SPlusShims;

namespace ICD.Connect.Devices.SPlusShims
{
	public abstract class AbstractSPlusDeviceBaseShim<TOriginator> : AbstractSPlusOriginatorShim<TOriginator>, ISPlusDeviceBaseShim<TOriginator> 
		where TOriginator : class, ISimplDeviceBase
	{
		/// <summary>
		/// Raised when the device goes online/offline.
		/// </summary>
		[PublicAPI("S+")]
		public event EventHandler<UShortEventArgs> OnIsOnlineStateChanged;

		/// <summary>
		/// This callback is raised when the shim wants the S+ class to re-send incoming data to the shim
		/// This is for syncronizing, for example, when an originator is attached.
		/// </summary>
		[PublicAPI("S+")]
		public event EventHandler OnResyncRequested;

		/// <summary>
		/// Returns true if the device hardware is detected by the system.
		/// </summary>
		[PublicAPI("S+")]
		public ushort IsOnline
		{
			get
			{
				TOriginator originator = Originator;
				if (originator == null)
					return 0;

				return (ushort)(originator.IsOnline ? 1 : 0);
			}
			set
			{
				TOriginator originator = Originator;
				if (originator == null)
					return;

				originator.SetIsOnline(value.ToBool());
			}
		}

		#region Private/Protected Methods

		/// <summary>
		/// Called when the originator is attached.
		/// Do any actions needed to syncronize
		/// </summary>
		protected override void InitializeOriginator()
		{
			base.InitializeOriginator();
			RequestResync();
		}

		/// <summary>
		/// Called when the originator is detached
		/// Do any actions needed to desyncronize
		/// </summary>
		protected override void DeinitializeOriginator()
		{
			base.DeinitializeOriginator();

			TOriginator originator = Originator;
			if (originator == null)
				return;

			originator.SetIsOnline(false);
		}

		protected virtual void RequestResync()
		{
			OnResyncRequested.Raise(this);
		}

		#endregion

		#region Originator Callbacks

		/// <summary>
		/// Subscribes to the originator events.
		/// </summary>
		/// <param name="originator"></param>
		protected override void Subscribe(TOriginator originator)
		{
			base.Subscribe(originator);

			if (originator == null)
				return;

			originator.OnIsOnlineStateChanged += OriginatorOnIsOnlineStateChanged;
			originator.OnRequestShimResync += OriginatorOnRequestShimResync;
		}

		/// <summary>
		/// Unsubscribes from the originator events.
		/// </summary>
		/// <param name="originator"></param>
		protected override void Unsubscribe(TOriginator originator)
		{
			base.Unsubscribe(originator);

			if (originator == null)
				return;

			originator.OnIsOnlineStateChanged -= OriginatorOnIsOnlineStateChanged;
			originator.OnRequestShimResync -= OriginatorOnRequestShimResync;
		}

		private void OriginatorOnIsOnlineStateChanged(object sender, DeviceBaseOnlineStateApiEventArgs eventArgs)
		{
			OnIsOnlineStateChanged.Raise(this, new UShortEventArgs(IsOnline));
		}

		private void OriginatorOnRequestShimResync(object sender, RequestShimResyncEventArgs requestShimResyncEventArgs)
		{
			RequestResync();
		}

		#endregion
	}
}
