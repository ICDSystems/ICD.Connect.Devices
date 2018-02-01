using ICD.Common.Utils;

namespace ICD.Connect.Devices.Controls
{
	public interface IVolumeRawLevelDeviceControl : IVolumeLevelDeviceControl
	{
		float VolumeRawMaxRange { get; }
		float VolumeRawMinRange { get; }
	}

	public static class VolumeRawLevelDeviceControlsExtensions
	{
		public static float ConvertRawToPosition(this IVolumeRawLevelDeviceControl control, float volumeRaw)
		{
			return MathUtils.MapRange(control.VolumeRawMinRange, control.VolumeRawMaxRange, 0.0f, 1.0f, volumeRaw);
		}

		public static float ConvertPositionToRaw(this IVolumeRawLevelDeviceControl control, float volumePosition)
		{
			return MathUtils.MapRange(0.0f, 1.0f, control.VolumeRawMinRange, control.VolumeRawMaxRange, volumePosition);
		}

		public static float ClampRawVolume(this IVolumeRawLevelDeviceControl control, float level)
		{
				return MathUtils.Clamp(level, control.VolumeRawMinRange, control.VolumeRawMaxRange);
		}
	}
}