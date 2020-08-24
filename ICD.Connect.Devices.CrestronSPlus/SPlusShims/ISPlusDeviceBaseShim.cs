using System;
using ICD.Common.Properties;
using ICD.Connect.Devices.CrestronSPlus.Devices.SPlus;
using ICD.Connect.Settings.CrestronSPlus.SPlusShims;

namespace ICD.Connect.Devices.CrestronSPlus.SPlusShims
{
	public interface ISPlusDeviceBaseShim<TOriginator> : ISPlusOriginatorShim<TOriginator> 
		where TOriginator : ISPlusDeviceBase
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