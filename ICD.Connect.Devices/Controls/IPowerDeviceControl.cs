using System;
using ICD.Common.Utils.EventArguments;
using ICD.Connect.API.Attributes;
using ICD.Connect.Devices.Proxies.Controls;

namespace ICD.Connect.Devices.Controls
{
	[ApiClass(typeof(ProxyPowerDeviceControl), typeof(IDeviceControl))]
	public interface IPowerDeviceControl : IDeviceControl
	{
		/// <summary>
		/// Raised when the powered state changes.
		/// </summary>
		[ApiEvent(PowerDeviceControlApi.EVENT_IS_POWERED, PowerDeviceControlApi.HELP_EVENT_IS_POWERED)]
		event EventHandler<BoolEventArgs> OnIsPoweredChanged;

		/// <summary>
		/// Gets the powered state of the device.
		/// </summary>
		[ApiProperty(PowerDeviceControlApi.PROPERTY_IS_POWERED, PowerDeviceControlApi.HELP_PROPERTY_IS_POWERED)]
		bool IsPowered { get; }

		/// <summary>
		/// Powers on the device.
		/// </summary>
		[ApiMethod(PowerDeviceControlApi.METHOD_POWER_ON, PowerDeviceControlApi.HELP_METHOD_POWER_ON)]
		void PowerOn();

		/// <summary>
		/// Powers off the device.
		/// </summary>
		[ApiMethod(PowerDeviceControlApi.METHOD_POWER_OFF, PowerDeviceControlApi.HELP_METHOD_POWER_OFF)]
		void PowerOff();
	}
}
