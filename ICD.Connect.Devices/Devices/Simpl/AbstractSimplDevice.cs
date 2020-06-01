using System;
using System.Collections.Generic;
using ICD.Common.Utils.EventArguments;
using ICD.Common.Utils.Extensions;
using ICD.Common.Utils.Services.Logging;
using ICD.Connect.API.Commands;
using ICD.Connect.API.Nodes;
using ICD.Connect.Devices.Controls;
using ICD.Connect.Devices.EventArguments;
using ICD.Connect.Settings;

namespace ICD.Connect.Devices.Simpl
{
	public abstract class AbstractSimplDevice<TSettings> : AbstractSimplDeviceBase<TSettings>, ISimplDevice
		where TSettings : ISimplDeviceSettings, new()
	{
		/// <summary>
		/// Raised when ControlsAvailable changes
		/// </summary>
		public event EventHandler<DeviceBaseControlsAvailableApiEventArgs> OnControlsAvailableChanged;

		/// <summary>
		/// Raised when the model changes.
		/// </summary>
		public event EventHandler<StringEventArgs> OnModelChanged;

		/// <summary>
		/// Raised when the serial number changes.
		/// </summary>
		public event EventHandler<StringEventArgs> OnSerialNumberChanged;

		private readonly DeviceControlsCollection m_Controls;

		private bool m_ControlsAvailable;
		private string m_Model;
		private string m_SerialNumber;

		#region Properties

		/// <summary>
		/// Gets the category for this originator type (e.g. Device, Port, etc)
		/// </summary>
		public override string Category { get { return "Device"; } }

		/// <summary>
		/// Gets if controls are available
		/// </summary>
		public bool ControlsAvailable
		{
			get { return m_ControlsAvailable; }
			private set
			{
				if (value == m_ControlsAvailable)
					return;

				m_ControlsAvailable = value;

				Logger.Set("Controls Available", eSeverity.Informational, ControlsAvailable);

				OnControlsAvailableChanged.Raise(this, new DeviceBaseControlsAvailableApiEventArgs(ControlsAvailable));
			}
		}

		/// <summary>
		/// Gets the discovered model.
		/// </summary>
		public string Model
		{
			get { return m_Model; }
			protected set
			{
				if (m_Model == value)
					return;

				m_Model = value;

				OnModelChanged.Raise(this, new StringEventArgs(value));
			}
		}

		/// <summary>
		/// Gets the discovered serial number.
		/// </summary>
		public string SerialNumber
		{
			get { return m_SerialNumber; }
			protected set
			{
				if (m_SerialNumber == value)
					return;

				m_SerialNumber = value;

				OnSerialNumberChanged.Raise(this, new StringEventArgs(value));
			}
		}

		/// <summary>
		/// Gets/sets the manufacturer for this device.
		/// </summary>
		public string ConfiguredManufacturer { get; set; }

		/// <summary>
		/// Gets/sets the model number for this device.
		/// </summary>
		public string ConfiguredModel { get; set; }

		/// <summary>
		/// Gets/sets the serial number for this device.
		/// </summary>
		public string ConfiguredSerialNumber { get; set; }

		/// <summary>
		/// Gets/sets the purchase date for this device.
		/// </summary>
		public DateTime ConfiguredPurchaseDate { get; set; }

		/// <summary>
		/// Gets the controls for this device.
		/// </summary>
		public DeviceControlsCollection Controls { get { return m_Controls; } }

		#endregion

		/// <summary>
		/// Constructor.
		/// </summary>
		protected AbstractSimplDevice()
		{
			m_Controls = new DeviceControlsCollection();
		}

		/// <summary>
		/// Release resources.
		/// </summary>
		protected override void DisposeFinal(bool disposing)
		{
			OnControlsAvailableChanged = null;
			OnModelChanged = null;
			OnSerialNumberChanged = null;

			m_Controls.Dispose();

			base.DisposeFinal(disposing);
		}

		#region Private Methods

		/// <summary>
		/// Gets the current state of Control Availability
		/// Default implementation is to follow IsOnline;
		/// </summary>
		/// <returns></returns>
		protected virtual bool GetControlsAvailable()
		{
			return IsOnline;
		}

		/// <summary>
		/// Override to handle the device going online/offline.
		/// </summary>
		/// <param name="isOnline"></param>
		protected override void HandleOnlineStateChange(bool isOnline)
		{
			base.HandleOnlineStateChange(isOnline);

			UpdateCachedControlsAvailable();
		}

		/// <summary>
		/// Updates the cached ControlsAvailable status and raises the OnControlsAvailableChanged if the cache chagnes
		/// </summary>
		protected virtual void UpdateCachedControlsAvailable()
		{
			ControlsAvailable = GetControlsAvailable();
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

			settings.Manufacturer = ConfiguredManufacturer;
			settings.Model = ConfiguredModel;
			settings.SerialNumber = ConfiguredSerialNumber;
			settings.PurchaseDate = ConfiguredPurchaseDate;
		}

		/// <summary>
		/// Override to apply settings to the instance.
		/// </summary>
		/// <param name="settings"></param>
		/// <param name="factory"></param>
		protected override void ApplySettingsFinal(TSettings settings, IDeviceFactory factory)
		{
			base.ApplySettingsFinal(settings, factory);

			ConfiguredManufacturer = settings.Manufacturer;
			ConfiguredModel = settings.Model;
			ConfiguredSerialNumber = settings.SerialNumber;
			ConfiguredPurchaseDate = settings.PurchaseDate;
		}

		/// <summary>
		/// Override to clear the instance settings.
		/// </summary>
		protected override void ClearSettingsFinal()
		{
			base.ClearSettingsFinal();

			ConfiguredManufacturer = null;
			ConfiguredModel = null;
			ConfiguredSerialNumber = null;
			ConfiguredPurchaseDate = DateTime.MinValue;
		}

		#endregion

		#region Console

		/// <summary>
		/// Gets the child console nodes.
		/// </summary>
		/// <returns></returns>
		public override IEnumerable<IConsoleNodeBase> GetConsoleNodes()
		{
			foreach (IConsoleNodeBase node in GetBaseConsoleNodes())
				yield return node;

			foreach (IConsoleNodeBase node in DeviceConsole.GetConsoleNodes(this))
				yield return node;
		}

		/// <summary>
		/// Wrokaround for "unverifiable code" warning.
		/// </summary>
		/// <returns></returns>
		private IEnumerable<IConsoleNodeBase> GetBaseConsoleNodes()
		{
			return base.GetConsoleNodes();
		}

		/// <summary>
		/// Calls the delegate for each console status item.
		/// </summary>
		/// <param name="addRow"></param>
		public override void BuildConsoleStatus(AddStatusRowDelegate addRow)
		{
			base.BuildConsoleStatus(addRow);

			DeviceConsole.BuildConsoleStatus(this, addRow);
		}

		/// <summary>
		/// Gets the child console commands.
		/// </summary>
		/// <returns></returns>
		public override IEnumerable<IConsoleCommand> GetConsoleCommands()
		{
			foreach (IConsoleCommand command in GetBaseConsoleCommands())
				yield return command;

			foreach (IConsoleCommand command in DeviceConsole.GetConsoleCommands(this))
				yield return command;
		}

		/// <summary>
		/// Workaround for "unverifiable code" warning.
		/// </summary>
		/// <returns></returns>
		private IEnumerable<IConsoleCommand> GetBaseConsoleCommands()
		{
			return base.GetConsoleCommands();
		}

		#endregion
	}
}
