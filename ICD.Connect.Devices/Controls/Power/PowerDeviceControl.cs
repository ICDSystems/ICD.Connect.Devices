using ICD.Connect.Devices.EventArguments;

namespace ICD.Connect.Devices.Controls.Power
{
	public sealed class PowerDeviceControl : AbstractPowerDeviceControl<IDeviceWithPower>
	{
		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="parent"></param>
		/// <param name="id"></param>
		public PowerDeviceControl(IDeviceWithPower parent, int id)
			: base(parent, id)
		{
			SetPowerState(parent.PowerState, 0);
		}

		#region Methods

		/// <summary>
		/// Override to implement the power-on action.
		/// </summary>
		protected override void PowerOnFinal()
		{
			Parent.PowerOn();
		}

		/// <summary>
		/// Override to implement the power-off action.
		/// </summary>
		protected override void PowerOffFinal()
		{
			Parent.PowerOff();
		}

		#endregion

		#region Parent Callbacks

		/// <summary>
		/// Subscribe to the parent events.
		/// </summary>
		/// <param name="parent"></param>
		protected override void Subscribe(IDeviceWithPower parent)
		{
			base.Subscribe(parent);

			parent.OnPowerStateChanged += ParentOnPowerStateChanged;
		}

		/// <summary>
		/// Unsubscribe from the parent events.
		/// </summary>
		/// <param name="parent"></param>
		protected override void Unsubscribe(IDeviceWithPower parent)
		{
			base.Unsubscribe(parent);

			parent.OnPowerStateChanged -= ParentOnPowerStateChanged;
		}

		/// <summary>
		/// Called when the parent power state changes.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="eventArgs"></param>
		private void ParentOnPowerStateChanged(object sender, PowerDeviceControlPowerStateApiEventArgs eventArgs)
		{
			SetPowerState(eventArgs.Data.PowerState, eventArgs.Data.ExpectedDuration);
		}

		#endregion
	}
}
