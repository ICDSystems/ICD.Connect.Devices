using System;
using ICD.Common.Properties;
using ICD.Connect.API.Attributes;
using ICD.Connect.Devices.EventArguments;
using ICD.Connect.Devices.Proxies.Controls;

namespace ICD.Connect.Devices.Controls.Power
{
	public enum ePowerState
	{
		Unknown,
		PowerOff,
		PowerOn,
		Warming,
		Cooling
	}

	public delegate void PrePowerDelegate([NotNull] Action powerCallback);

	[ApiClass(typeof(ProxyPowerDeviceControl), typeof(IDeviceControl))]
	public interface IPowerDeviceControl : IDeviceControl
	{
		/// <summary>
		/// Raised when the powered state changes.
		/// </summary>
		[ApiEvent(PowerDeviceControlApi.EVENT_POWER_STATE, PowerDeviceControlApi.HELP_EVENT_POWER_STATE)]
		event EventHandler<PowerDeviceControlPowerStateApiEventArgs> OnPowerStateChanged;

		#region Properties

		/// <summary>
		/// Gets the powered state of the device.
		/// </summary>
		[ApiProperty(PowerDeviceControlApi.PROPERTY_POWER_STATE, PowerDeviceControlApi.HELP_PROPERTY_POWER_STATE)]
		ePowerState PowerState { get; }

		/// <summary>
		/// Gets/sets the delegate to execute before powering the device.
		/// </summary>
		PrePowerDelegate PrePowerOn { get; set; }

		/// <summary>
		/// Gets/sets the delegate to execute before powering off the device.
		/// </summary>
		PrePowerDelegate PrePowerOff { get; set; }

		#endregion

		#region Methods

		/// <summary>
		/// Powers on the device.
		/// </summary>
		[ApiMethod(PowerDeviceControlApi.METHOD_POWER_ON, PowerDeviceControlApi.HELP_METHOD_POWER_ON)]
		void PowerOn();
		
		/// <summary>
		/// Powers on the device.
		/// </summary>
		/// /// <param name="bypassPrePowerOn">If true, skips the pre power on delegate.</param>
		[ApiMethod(PowerDeviceControlApi.METHOD_POWER_ON_BYPASS, PowerDeviceControlApi.HELP_METHOD_POWER_ON_BYPASS)]
		void PowerOn(bool bypassPrePowerOn);

		/// <summary>
		/// Powers off the device.
		/// </summary>
		[ApiMethod(PowerDeviceControlApi.METHOD_POWER_OFF, PowerDeviceControlApi.HELP_METHOD_POWER_OFF)]
		void PowerOff();
		
		/// <summary>
		/// Powers off the device.
		/// </summary>
		/// <param name="bypassPostPowerOff">If true, skips the post power off delegate.</param>
		[ApiMethod(PowerDeviceControlApi.METHOD_POWER_OFF_BYPASS, PowerDeviceControlApi.HELP_METHOD_POWER_OFF_BYPASS)]
		void PowerOff(bool bypassPostPowerOff);

		#endregion
	}
}
