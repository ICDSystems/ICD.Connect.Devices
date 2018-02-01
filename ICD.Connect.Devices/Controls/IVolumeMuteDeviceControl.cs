using System;
using ICD.Common.Properties;
using ICD.Common.Utils;
using ICD.Common.Utils.EventArguments;

namespace ICD.Connect.Devices.Controls
{
	/// <summary>
	/// IVolumeMuteDeviceControl is for devices that offer both toggle and direct set for mute state
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
