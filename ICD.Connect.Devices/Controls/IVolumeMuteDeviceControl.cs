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
	public interface IVolumeMuteDeviceControl : IVolumeMuteBasicDeviceControl
	{

		#region Methods

		/// <summary>
		/// Sets the mute state.
		/// </summary>
		/// <param name="mute"></param>
		[PublicAPI]
		void SetVolumeMute(bool mute);

		#endregion
	}
}
