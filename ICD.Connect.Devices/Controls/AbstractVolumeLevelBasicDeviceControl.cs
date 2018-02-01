using System;
using ICD.Common.Properties;
using ICD.Common.Utils;
using ICD.Connect.Devices.Timers;

namespace ICD.Connect.Devices.Controls
{
	public abstract class AbstractVolumeLevelBasicDeviceControl<T> : AbstractDeviceControl<T>, IVolumeLevelBasicDeviceControl
		where T : IDeviceBase
	{
		private const int DEFAULT_REPEAT_TIME_INITIAL = 250;
		private const int DEFAULT_REPEAT_TIME_RECURRING = 250;
		private const float FLOAT_COMPARE_TOLERANCE = 0.00001F;

		private VolumeBasicRepeater m_Repeater;

		private readonly SafeCriticalSection m_RepeaterCriticalSection;

		private int? m_RepeatTimeInitial;

		private int? m_RepeatTimeRecurring;

		/// <summary>
		/// Time from the press to the first repeat
		/// </summary>
		[PublicAPI]
		public virtual int RepeatTimeInitial
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
		public virtual int RepeatTimeRecurring
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
		public abstract void VolumeLevelIncrement();

		/// <summary>
		/// Volume Level Decrement
		/// </summary>
		public abstract void VolumeLevelDecrement();

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
					m_Repeater = new VolumeBasicRepeater(RepeatTimeInitial, RepeatTimeRecurring);
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
		protected AbstractVolumeLevelBasicDeviceControl(T parent, int id) : base(parent, id)
		{
			m_RepeaterCriticalSection = new SafeCriticalSection();
		}
	}
}
