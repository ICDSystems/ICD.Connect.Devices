using System;
using ICD.Common.Properties;
using ICD.Common.Utils;
using ICD.Connect.Devices.Timers;

namespace ICD.Connect.Devices.Controls
{
	public abstract class AbstractVolumeLevelDeviceControl<T> : AbstractDeviceControl<T>, IVolumeLevelDeviceControl
		where T : IDeviceBase
	{
		private const int DEFAULT_REPEAT_TIME_INITIAL = 250;
		private const int DEFAULT_REPEAT_TIME_RECURRING = 250;
		private const float FLOAT_COMPARE_TOLERANCE = 0.00001F;
		private const float DEFAULT_INCREMENT_VALUE = 1;

		private float? m_IncrementValue;

		private VolumeRepeater m_Repeater;

		private readonly SafeCriticalSection m_RepeaterCriticalSection;

		private int? m_RepeatTimeInitial;

		private int? m_RepeatTimeRecurring;
		private float? m_VolumeRawMax;
		private float? m_VolumeRawMin;

		protected float IncrementValue
		{
			get
			{
				if (m_IncrementValue != null)
					return (float)m_IncrementValue;
				return DEFAULT_INCREMENT_VALUE;
			}
			set
			{
				if (Math.Abs(value) > FLOAT_COMPARE_TOLERANCE)
					m_IncrementValue = value;
				else
					m_IncrementValue = null;
			}
		}

		public virtual float? VolumeRawMax
		{
			get
			{
				return m_VolumeRawMax;
			}
			set
			{
				m_VolumeRawMax = value;
				if (m_VolumeRawMax != null)
					ClampLevel();

			}
		}

		public virtual float? VolumeRawMin
		{
			get
			{
				return m_VolumeRawMin;
			}
			set
			{
				m_VolumeRawMin = value;
				if (m_VolumeRawMin != null)
					ClampLevel();
			}
		}

		/// <summary>
		/// Time from the press to the first repeat
		/// </summary>
		[PublicAPI]
		public int RepeatTimeInitial
		{
			get
			{
				if (m_RepeatTimeInitial != null)
					return (int)m_RepeatTimeInitial;
				return DEFAULT_REPEAT_TIME_INITIAL;
			}
			set
			{
				if (Math.Abs(value) > FLOAT_COMPARE_TOLERANCE)
					m_RepeatTimeInitial = value;
				else
					m_RepeatTimeInitial = null;
			}
		}

		/// <summary>
		/// Time from the first repeat to subsequent repeats
		/// </summary>
		[PublicAPI]
		public int RepeatTimeRecurring
		{
			get
			{
				if (m_RepeatTimeRecurring != null)
					return (int)m_RepeatTimeRecurring;
				return DEFAULT_REPEAT_TIME_RECURRING;
			}
			set
			{
				if (Math.Abs(value) > FLOAT_COMPARE_TOLERANCE)
					m_RepeatTimeRecurring = value;
				else
					m_RepeatTimeRecurring = null;
			}
		}

		/// <summary>
		/// Volume Level Increment
		/// </summary>
		public virtual void VolumeLevelIncrement()
		{
			VolumeLevelIncrement(IncrementValue);
		}

		/// <summary>
		/// Volume Level Decrement
		/// </summary>
		public virtual void VolumeLevelDecrement()
		{
			VolumeLevelDecrement(IncrementValue);
		}

		/// <summary>
		/// Starts a volume ramp up operation
		/// VolumeLevelRampStop() must be called to stop the ramping
		/// </summary>
		public void VolumeLevelRampUp()
		{
			VolumeLevelRamp(true);
		}

		/// <summary>
		/// Starts a volume ramp down operation
		/// VolumeLevelRampStop() must be called to stop the ramping
		/// </summary>
		public void VolumeLevelRampDown()
		{
			VolumeLevelRamp(false);
		}

		/// <summary>
		/// Stops the volume ramp and disposes of the repeater timer
		/// </summary>
		public void VolumeLevelRampStop()
		{
			m_RepeaterCriticalSection.Enter();
			try
			{
				if (m_Repeater == null)
					return;

				m_Repeater.Release();
				m_Repeater.Dispose();
				m_Repeater = null;
			}
			finally
			{
				m_RepeaterCriticalSection.Leave();
			}
		}

		/// <summary>
		/// Creates the repeater timer and starts up/down ramp
		/// </summary>
		/// <param name="up">true for up ramp, false for down ramp</param>
		private void VolumeLevelRamp(bool up)
		{
			m_RepeaterCriticalSection.Enter();
			try
			{
				if (m_Repeater == null)
				{
					m_Repeater = new VolumeRepeater(RepeatTimeInitial, RepeatTimeRecurring);
					m_Repeater.SetControl(this);
				}
				else
					m_Repeater.Release();

				m_Repeater.VolumeHold(up);
			}
			finally
			{
				m_RepeaterCriticalSection.Leave();
			}
		}

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="parent">Device this control belongs to</param>
		/// <param name="id">Id of this control in the device</param>
		protected AbstractVolumeLevelDeviceControl(T parent, int id) : base(parent, id)
		{
			m_RepeaterCriticalSection = new SafeCriticalSection();
		}

		public abstract event EventHandler<VolumeDeviceVolumeChangedEventArgs> OnVolumeChanged;

		public abstract float VolumeRaw { get; }
		public abstract float VolumePosition { get; }

		public virtual string VolumeString
		{
			get { return VolumeRaw.ToString("n2"); }
		}

		protected void ClampLevel()
		{
			float clampValue = this.ClampRawVolume(VolumeRaw);
			if (Math.Abs(clampValue - VolumeRaw) > FLOAT_COMPARE_TOLERANCE)
				SetVolumeRaw(clampValue);
		}

		public abstract void SetVolumeRaw(float volume);

		public abstract void SetVolumePosition(float position);

		public virtual void VolumeLevelIncrement(float incrementValue)
		{
			SetVolumeRaw(this.ClampRawVolume(VolumeRaw + incrementValue));
		}

		public virtual void VolumeLevelDecrement(float decrementValue)
		{
			SetVolumeRaw(this.ClampRawVolume(VolumeRaw - decrementValue));
		}
	}
}
