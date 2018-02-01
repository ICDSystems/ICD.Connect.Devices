using System;
using ICD.Common.Properties;
using ICD.Common.Utils;
using ICD.Common.Utils.EventArguments;

namespace ICD.Connect.Devices.Controls
{
	/// <summary>
	/// IVolumeDeviceControls are used with devices that have volume and mute features.
	/// For example, a TV or a Codec.
	/// </summary>
	public interface IVolumeMuteFeedbackDeviceControl : IVolumeMuteDeviceControl
	{
		/// <summary>
		/// Raised when the mute state changes.
		/// </summary>
		[PublicAPI]
		event EventHandler<BoolEventArgs> OnMuteStateChanged;

		#region Properties

		/// <summary>
		/// Gets the muted state.
		/// </summary>
		[PublicAPI]
		bool VolumeIsMuted { get; }

		#endregion
	}
}
