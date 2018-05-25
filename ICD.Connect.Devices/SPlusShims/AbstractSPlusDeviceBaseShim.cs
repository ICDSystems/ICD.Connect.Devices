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

				originator.IsOnline = value.ToBool();
			}
		}

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
		}

		private void OriginatorOnIsOnlineStateChanged(object sender, DeviceBaseOnlineStateApiEventArgs eventArgs)
		{
			OnIsOnlineStateChanged.Raise(this, new UShortEventArgs(IsOnline));
		}

		#endregion
	}
}
