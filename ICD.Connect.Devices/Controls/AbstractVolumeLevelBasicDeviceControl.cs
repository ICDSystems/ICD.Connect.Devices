﻿using System;
using ICD.Common.Properties;
using ICD.Common.Utils;
using ICD.Connect.Devices.Timers;

namespace ICD.Connect.Devices.Controls
{
	public abstract class AbstractVolumeLevelBasicDeviceControl<T> : AbstractDeviceControl<T>, IVolumeLevelBasicDeviceControl
		where T : IDeviceBase
	{

		#region Constants
		/// <summary>
		/// Default time before repeat for volume ramping operations
		/// </summary>
		private const int DEFAULT_REPEAT_BEFORE_TIME = 250;

		/// <summary>
		/// Default time between repeats for volume ramping operations
		/// </summary>
		private const int DEFAULT_REPEAT_BETWEEN_TIME = 250;

		/// <summary>
		/// Tolerance for float comparisons
		/// </summary>
		private const float FLOAT_COMPARE_TOLERANCE = 0.00001F;
		#endregion

		#region Fields
		/// <summary>
		/// Repeater for volume ramping operaions
		/// </summary>
		private VolumeBasicRepeater m_Repeater;

		/// <summary>
		/// Used when creating/accessing/disposing repeater
		/// </summary>
		private readonly SafeCriticalSection m_RepeaterCriticalSection;

		private int? m_RepeatBeforeTime;

		private int? m_RepeatBetweenTime;

		#endregion

		#region Properties

		/// <summary>
		/// Time from the press to the repeat
		/// </summary>
		[PublicAPI]
		public virtual int RepeatBeforeTime
		{
			get
			{
				if (m_RepeatBeforeTime != null)
					return (int)m_RepeatBeforeTime;
				return DEFAULT_REPEAT_BEFORE_TIME;
			}
			set
			{
				if (Math.Abs(value) > FLOAT_COMPARE_TOLERANCE)
					m_RepeatBeforeTime = value;
				else
					m_RepeatBeforeTime = null;
			}
		}

		/// <summary>
		/// Time between repeats
		/// </summary>
		[PublicAPI]
		public virtual int RepeatBetweenTime
		{
			get
			{
				if (m_RepeatBetweenTime != null)
					return (int)m_RepeatBetweenTime;
				return DEFAULT_REPEAT_BETWEEN_TIME;
			}
			set
			{
				if (Math.Abs(value) > FLOAT_COMPARE_TOLERANCE)
					m_RepeatBetweenTime = value;
				else
					m_RepeatBetweenTime = null;
			}
		}

		#endregion

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="parent">Device this control belongs to</param>
		/// <param name="id">Id of this control in the device</param>
		protected AbstractVolumeLevelBasicDeviceControl(T parent, int id) : base(parent, id)
		{
			m_RepeaterCriticalSection = new SafeCriticalSection();
		}

		#region Methods

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

		#endregion

		#region Private Methods

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
					m_Repeater = new VolumeBasicRepeater(RepeatBeforeTime, RepeatBetweenTime);
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

		#endregion
	}
}
