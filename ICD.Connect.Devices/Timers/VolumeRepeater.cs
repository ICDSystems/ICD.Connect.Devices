﻿using System;
using ICD.Connect.Devices.Controls;

namespace ICD.Connect.Devices.Timers
{
	/// <summary>
	/// VolumeRepeater allows for a virtual "button" to be held, raising a callback for
	/// every repeat interval.
	/// </summary>
	public sealed class VolumeRepeater : AbstactVolumeRepeater
	{
		private IVolumeLevelDeviceControl m_Control;

		private readonly float? m_InitialIncrement;
		private readonly float? m_RepeatIncrement;

		#region Constructor

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="initialIncrement">The increment when the repeater is first held</param>
		/// <param name="repeatIncrement">The increment for every subsequent repeat</param>
		/// <param name="beforeRepeat">The delay before the second increment</param>
		/// <param name="betweenRepeat">The delay between each subsequent repeat</param>
		public VolumeRepeater(float? initialIncrement, float? repeatIncrement, int beforeRepeat, int betweenRepeat)
			: base(beforeRepeat, betweenRepeat)
		{
			m_InitialIncrement = initialIncrement;
			m_RepeatIncrement = repeatIncrement;
		}

		/// <summary>
		/// Constructor.  Uses device's Increment methods
		/// </summary>
		/// <param name="beforeRepeat">The delay before the second increment</param>
		/// <param name="betweenRepeat">The delay between each subsequent repeat</param>
		public VolumeRepeater(int beforeRepeat, int betweenRepeat)
			: base(beforeRepeat, betweenRepeat)
		{
			m_InitialIncrement = null;
			m_RepeatIncrement = null;
		}

		/// <summary>
		/// Destructor.
		/// </summary>
		~VolumeRepeater()
		{
			Dispose();
		}

		#endregion

		#region Methods

		/// <summary>
		/// Sets the control.
		/// </summary>
		/// <param name="control"></param>
		public void SetControl(IVolumeLevelDeviceControl control)
		{
			m_Control = control;
		}

		#endregion

		#region Private Methods

		/// <summary>
		/// Performs the increment for the initial press
		/// </summary>
		protected override void IncrementVolumeInitial()
		{
			if (m_InitialIncrement != null)
				IncrementVolume((float)m_InitialIncrement);
			else
				IncrementVolume();
		}

		/// <summary>
		/// Performs the increment for the repeats
		/// </summary>
		protected override void IncrementVolumeSubsequent()
		{
			if (m_RepeatIncrement != null)
				IncrementVolume((float)m_RepeatIncrement);
			else
				IncrementVolume();
		}

		/// <summary>
		/// Adjusts the device volume by the specified increment.
		/// Applies Up/Down offset based on Up property value.
		/// </summary>
		/// <param name="increment"></param>
		private void IncrementVolume(float increment)
		{
			if (m_Control == null)
				throw new InvalidOperationException("Can't increment volume without control set");

			float delta = Up ? increment : -1 * increment;
			float newVolume = m_Control.ClampRawVolume(m_Control.VolumeRaw + delta);

			m_Control.SetVolumeRaw(newVolume);
		}

		/// <summary>
		/// Adjusts the device volume using the default Increment/Decremnet methods.
		/// Determines Increment/Decrement bad on Up property value.
		/// </summary>
		private void IncrementVolume()
		{
			if (m_Control == null)
				throw new InvalidOperationException("Can't increment volume without control set");

			if (Up)
				m_Control.VolumeLevelIncrement();
			else
				m_Control.VolumeLevelDecrement();
		}

		#endregion
	}
}
