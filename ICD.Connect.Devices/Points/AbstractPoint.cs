using System;
using System.Collections.Generic;
using ICD.Common.Properties;
using ICD.Common.Utils.Extensions;
using ICD.Common.Utils.Services.Logging;
using ICD.Connect.API.Nodes;
using ICD.Connect.Devices.Controls;
using ICD.Connect.Devices.EventArguments;
using ICD.Connect.Devices.Extensions;
using ICD.Connect.Settings;
using ICD.Connect.Settings.Originators;

namespace ICD.Connect.Devices.Points
{
	public abstract class AbstractPoint<TSettings, TControl> : AbstractOriginator<TSettings>, IPoint
		where TSettings : IPointSettings, new()
		where TControl : class, IDeviceControl
	{
		/// <summary>
		/// Raised when the wrapped control changes.
		/// </summary>
		public event EventHandler<DeviceControlEventArgs> OnControlChanged;

		#region Properties

		/// <summary>
		/// Gets/sets the control for this point.
		/// </summary>
		[CanBeNull]
		IDeviceControl IPoint.Control { get { return Control; } }

		/// <summary>
		/// Gets/sets the control for this point.
		/// </summary>
		[CanBeNull]
		public TControl Control { get; private set; }

		/// <summary>
		/// Gets the device ID.
		/// </summary>
		public int DeviceId { get; private set; }

		/// <summary>
		/// Gets the control ID.
		/// </summary>
		public int ControlId { get; private set; }

		#endregion

		#region Methods

		/// <summary>
		/// Sets the wrapped control.
		/// </summary>
		/// <param name="control"></param>
		void IPoint.SetControl(IDeviceControl control)
		{
			SetControl((TControl)control);
		}

		/// <summary>
		/// Sets the wrapped control.
		/// </summary>
		/// <param name="control"></param>
		public void SetControl(TControl control)
		{
			if (control == Control)
				return;

			Unsubscribe(Control);

			Control = control;
			if (Control != null)
			{
				DeviceId = Control.Parent.Id;
				ControlId = Control.Id;
			}

			Subscribe(Control);

			OnControlChanged.Raise(this, new DeviceControlEventArgs(control));
		}

		#endregion

		#region Control Callbacks

		/// <summary>
		/// Override to handle unsubscribing to the control events.
		/// </summary>
		/// <param name="control"></param>
		protected virtual void Subscribe([CanBeNull] TControl control)
		{
			if (control == null)
				return;

			control.Parent.Controls.OnControlAdded += ControlsOnControlAdded;
			control.Parent.Controls.OnControlRemoved += ControlsOnControlRemoved;
		}

		/// <summary>
		/// Override to handle unsubscribing from the control events.
		/// </summary>
		/// <param name="control"></param>
		protected virtual void Unsubscribe([CanBeNull] TControl control)
		{
			if (control == null)
				return;

			control.Parent.Controls.OnControlAdded -= ControlsOnControlAdded;
			control.Parent.Controls.OnControlRemoved -= ControlsOnControlRemoved;
		}

		/// <summary>
		/// Called when a control is added to the parent device controls collection.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="eventArgs"></param>
		private void ControlsOnControlAdded(object sender, DeviceControlEventArgs eventArgs)
		{
			// Wrap the new control if it matches the device id and control id
			TControl control = eventArgs.Data as TControl;
			if (control != null && control.Parent.Id == DeviceId && control.Id == ControlId)
				SetControl(control);
		}

		/// <summary>
		/// Called when a control is removed from the parent device controls collection.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="eventArgs"></param>
		private void ControlsOnControlRemoved(object sender, DeviceControlEventArgs eventArgs)
		{
			// Clear this control if it was removed from the parent device
			if (eventArgs.Data == Control)
				SetControl(null);
		}

		#endregion

		#region Settings

		/// <summary>
		/// Override to apply properties to the settings instance.
		/// </summary>
		/// <param name="settings"></param>
		protected override void CopySettingsFinal(TSettings settings)
		{
			base.CopySettingsFinal(settings);

			settings.DeviceId = DeviceId;
			settings.ControlId = ControlId;
		}

		/// <summary>
		/// Override to apply settings to the instance.
		/// </summary>
		/// <param name="settings"></param>
		/// <param name="factory"></param>
		protected override void ApplySettingsFinal(TSettings settings, IDeviceFactory factory)
		{
			base.ApplySettingsFinal(settings, factory);

			DeviceId = settings.DeviceId;
			ControlId = settings.ControlId;

			IDeviceBase device = null;
			TControl control = null;

			if (settings.DeviceId == 0)
				Log(eSeverity.Error, "No device id configured");
			else
			{
				try
				{
					device = factory.GetDeviceById(settings.DeviceId);
				}
				catch (KeyNotFoundException)
				{
					Log(eSeverity.Error, "No device with id {0}", settings.DeviceId);
				}
			}

			if (device != null)
			{
				try
				{
					control = device.Controls.GetControl<TControl>(settings.ControlId);
				}
				catch (Exception e)
				{
					Log(eSeverity.Error, "Unable to get control {0} from device {1} - {2}", settings.ControlId,
					    settings.DeviceId, e.Message);
				}
			}

			SetControl(control);
		}

		/// <summary>
		/// Override to clear the instance settings.
		/// </summary>
		protected override void ClearSettingsFinal()
		{
			base.ClearSettingsFinal();

			Control = null;
			DeviceId = 0;
			ControlId = 0;
		}

		#endregion

		#region Console

		/// <summary>
		/// Calls the delegate for each console status item.
		/// </summary>
		/// <param name="addRow"></param>
		public override void BuildConsoleStatus(AddStatusRowDelegate addRow)
		{
			base.BuildConsoleStatus(addRow);

			addRow("Control", Control);
			addRow("DeviceId", DeviceId);
			addRow("ControlId", ControlId);
		}

		#endregion
	}
}
