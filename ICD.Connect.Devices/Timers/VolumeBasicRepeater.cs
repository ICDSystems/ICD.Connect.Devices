﻿using System;
using ICD.Connect.Devices.Controls;

namespace ICD.Connect.Devices.Timers
{
	/// <summary>
	/// VolumeRepeater allows for a virtual "button" to be held, raising a callback for
	/// every repeat interval.
	/// </summary>
	public sealed class VolumeBasicRepeater : AbstactVolumeRepeater
	{
		private IVolumeLevelBasicDeviceControl m_Control;

		#region Constructor

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="beforeRepeat">The delay before the second increment</param>
		/// <param name="betweenRepeat">The delay between each subsequent repeat</param>
		public VolumeBasicRepeater(int beforeRepeat, int betweenRepeat) : base(beforeRepeat, betweenRepeat)
		{
		}

		/// <summary>
		/// Destructor.
		/// </summary>
		~VolumeBasicRepeater()
		{
			Dispose();
		}

		#endregion

		#region Methods

		/// <summary>
		/// Sets the control.
		/// </summary>
		/// <param name="control"></param>
		public void SetControl(IVolumeLevelBasicDeviceControl control)
		{
			m_Control = control;
		}

		#endregion

		#region Private Methods

		protected override void IncrementVolumeInitial()
		{
			IncrementVolume();
		}

		protected override void IncrementVolumeSubsequent()
		{
			IncrementVolume();
		}

		/// <summary>
		/// Adjusts the device volume.
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
