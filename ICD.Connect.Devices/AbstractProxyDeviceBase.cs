using System;
using ICD.Common.Utils.EventArguments;
using ICD.Connect.Devices.Controls;
using ICD.Connect.Settings;

namespace ICD.Connect.Devices
{
    public abstract class AbstractProxyDeviceBase : AbstractProxyOriginator, IProxyDeviceBase
    {
		public event EventHandler<BoolEventArgs> OnIsOnlineStateChanged;

		private readonly DeviceControlsCollection m_Controls;

		#region Properties

		public bool IsOnline { get { return true; } }

		/// <summary>
		/// Gets the controls for this device.
		/// </summary>
		public DeviceControlsCollection Controls { get { return m_Controls; } }

		#endregion

		/// <summary>
		/// Constructor.
		/// </summary>
		protected AbstractProxyDeviceBase()
		{
			m_Controls = new DeviceControlsCollection();
		}

		/// <summary>
		/// Release resources.
		/// </summary>
		/// <param name="disposing"></param>
		protected override void DisposeFinal(bool disposing)
		{
			OnIsOnlineStateChanged = null;

			base.DisposeFinal(disposing);
		}
	}
}
