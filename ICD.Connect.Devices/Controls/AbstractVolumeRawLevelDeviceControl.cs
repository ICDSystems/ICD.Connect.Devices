using System;
using ICD.Common.Properties;
using ICD.Common.Utils;
using ICD.Connect.Devices.Timers;

namespace ICD.Connect.Devices.Controls
{
	public abstract class AbstractVolumeRawLevelDeviceControl<T> : AbstractVolumeLevelDeviceControl<T>, IVolumeRawLevelDeviceControl where T : IDeviceBase
	{
		

		protected abstract float VolumeRawMinAbsolute { get; }

		protected abstract float VolumeRawMaxAbsolute { get; }

		public float VolumeRawMinRange
		{
			get
			{
				return VolumeRawMin == null ? VolumeRawMinAbsolute : Math.Max(VolumeRawMinAbsolute, (float)VolumeRawMin);
			}
		}

		public float VolumeRawMaxRange
		{
			get
			{
				return VolumeRawMax == null ? VolumeRawMaxAbsolute : Math.Min(VolumeRawMaxAbsolute, (float)VolumeRawMax);
			}
		}

		


		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="parent">Device this control belongs to</param>
		/// <param name="id">Id of this control in the device</param>
		protected AbstractVolumeRawLevelDeviceControl(T parent, int id) : base(parent, id)
		{
		}

		public override float VolumePosition
		{
			get { return this.ConvertRawToPosition(VolumeRaw); }
		}

		public override void SetVolumePosition(float position)
		{
			SetVolumeRaw(this.ConvertPositionToRaw(position));
		}
		
	}
}
