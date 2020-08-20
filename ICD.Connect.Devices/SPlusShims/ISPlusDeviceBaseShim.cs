using System;
using ICD.Common.Properties;
using ICD.Connect.Devices.Simpl;
using ICD.Connect.Settings.SPlusShims;

namespace ICD.Connect.Devices.SPlusShims
{
	public interface ISPlusDeviceBaseShim<TOriginator> : ISPlusOriginatorShim<TOriginator> 
		where TOriginator : ISimplDeviceBase
	{
		/// <summary>
		/// Gets/sets the online status of the device.
		/// </summary>
		[PublicAPI("S+")]
		ushort IsOnline { get; set; }

		/// <summary>
		/// This callback is raised when the shim wants the S+ class to re-send incoming data to the shim
		/// This is for syncronizing, for example, when an originator is attached.
		/// </summary>
		[PublicAPI("S+")]
		event EventHandler OnResyncRequested;
	}
}