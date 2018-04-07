using System;
using ICD.Common.Properties;
using ICD.Common.Utils;
using ICD.Common.Utils.EventArguments;
using ICD.Connect.API.Attributes;
using ICD.Connect.Devices.Controls;
using ICD.Connect.Devices.Proxies.Devices;
using ICD.Connect.Settings;

namespace ICD.Connect.Devices
{
	[ApiClass(typeof(ProxyDeviceBase), typeof(IOriginator))]
	public interface IDeviceBase : IOriginator, IStateDisposable
	{
		/// <summary>
		/// Raised when the device goes online/offline.
		/// </summary>
		[ApiEvent(DeviceBaseApi.EVENT_IS_ONLINE, DeviceBaseApi.HELP_EVENT_IS_ONLINE)]
		event EventHandler<BoolEventArgs> OnIsOnlineStateChanged;

		/// <summary>
		/// Returns true if the device hardware is detected by the system.
		/// </summary>
		[ApiProperty(DeviceBaseApi.PROPERTY_IS_ONLINE, DeviceBaseApi.HELP_PROPERTY_IS_ONLINE)]
		bool IsOnline { get; }

		/// <summary>
		/// Gets the controls for this device.
		/// </summary>
		[ApiNodeGroup(DeviceBaseApi.NODE_GROUP_CONTROLS, DeviceBaseApi.HELP_NODE_GROUP_CONTROLS)]
		DeviceControlsCollection Controls { get; }
	}
}
