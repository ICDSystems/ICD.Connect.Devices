namespace ICD.Connect.Devices.Controls
{
    public interface IVolumeMuteBasicDeviceControl : IVolumeDeviceControl
    {
		/// <summary>
		/// Toggles the current mute state.
		/// </summary>
	    void VolumeMuteToggle();
    }
}
