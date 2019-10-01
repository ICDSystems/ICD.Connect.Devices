﻿using ICD.Connect.Settings;
using ICD.Connect.Settings.Originators;

namespace ICD.Connect.Devices.Points
{
	public abstract class AbstractPoint<TSettings> : AbstractOriginator<TSettings>, IPoint
		where TSettings : IPointSettings, new()
	{
		/// <summary>
		/// Device id
		/// </summary>
		public int DeviceId { get; set; }

		/// <summary>
		/// Control id.
		/// </summary>
		public int ControlId { get; set; }

		#region Settings

		protected override void CopySettingsFinal(TSettings settings)
		{
			base.CopySettingsFinal(settings);

			settings.DeviceId = DeviceId;
			settings.ControlId = ControlId;
		}

		protected override void ApplySettingsFinal(TSettings settings, IDeviceFactory factory)
		{
			base.ApplySettingsFinal(settings, factory);

			DeviceId = settings.DeviceId;
			ControlId = settings.ControlId;
		}

		protected override void ClearSettingsFinal()
		{
			base.ClearSettingsFinal();

			DeviceId = 0;
			ControlId = 0;
		}

		#endregion
	}
}
