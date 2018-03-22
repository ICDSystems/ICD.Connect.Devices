using System;
using ICD.Common.Utils.EventArguments;
using ICD.Connect.API.Attributes;
using ICD.Connect.API.Nodes;
using ICD.Connect.Devices.Controls;
using ICD.Connect.Settings;

namespace ICD.Connect.Devices.Proxies
{
    public abstract class AbstractProxyDeviceBase : AbstractProxyOriginator, IProxyDeviceBase
    {
		public event EventHandler<BoolEventArgs> OnIsOnlineStateChanged;

		private readonly DeviceControlsCollection m_Controls;

		#region Properties

		[ApiProperty("IsOnline", "Gets the online state of the device.")]
		public bool IsOnline { get { return true; } }

		/// <summary>
		/// Gets the controls for this device.
		/// </summary>
		public DeviceControlsCollection Controls { get { return m_Controls; } }

		[ApiNodeGroup("Controls", "The controls for this device.")]
		private IApiNodeGroup ApiControls { get; set; }

		#endregion

		/// <summary>
		/// Constructor.
		/// </summary>
		protected AbstractProxyDeviceBase()
		{
			m_Controls = new DeviceControlsCollection();
			ApiControls = new ApiControlsNodeGroup(m_Controls);
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
