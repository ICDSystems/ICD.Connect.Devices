﻿using System;
using ICD.Common.Utils.EventArguments;
using ICD.Common.Utils.Extensions;
using ICD.Connect.API.Nodes;
using ICD.Connect.Devices.Telemetry.DeviceInfo.Abstract;
using ICD.Connect.Devices.Telemetry.DeviceInfo.Configured.Settings;

namespace ICD.Connect.Devices.Telemetry.DeviceInfo.Configured
{
	public sealed class ConfiguredDeviceInfo : AbstractDeviceInfo<IConfiguredNetworkDeviceInfo>, IConfiguredDeviceInfo
	{
		#region Fields

		private DateTime? m_PurchaseDate;

		#endregion

		#region Events

		public override event EventHandler<StringEventArgs> OnMakeChanged;
		public override event EventHandler<StringEventArgs> OnModelChanged;
		public override event EventHandler<StringEventArgs> OnSerialNumberChanged;

		public event EventHandler<DateTimeNullableEventArgs> OnPurchaseDateChanged;

		#endregion

		#region Properties

		public DateTime? PurchaseDate
		{
			get { return m_PurchaseDate; }
			set
			{
				if (value == m_PurchaseDate)
					return;

				m_PurchaseDate = value;

				OnPurchaseDateChanged.Raise(this, new DateTimeNullableEventArgs(value));
			}
		}

		#endregion

		#region Constructor

		/// <summary>
		/// Constructor.
		/// </summary>
		public ConfiguredDeviceInfo()
			: base(new ConfiguredNetworkDeviceInfo())
		{
		}

		#endregion

		#region Settings

		/// <summary>
		/// Apply the configuration from the settings
		/// </summary>
		/// <param name="settings"></param>
		public void ApplySettings(ConfiguredDeviceInfoSettings settings)
		{
			Make = settings.Make;
			Model = settings.Model;
			SerialNumber = settings.SerialNumber;
			PurchaseDate = settings.PurchaseDate;
			NetworkInfo.ApplySettings(settings.NetworkInfo);
		}

		/// <summary>
		/// Copy the configuration to the given settings
		/// </summary>
		/// <param name="settings"></param>
		public void CopySettings(ConfiguredDeviceInfoSettings settings)
		{
			settings.Make = Make;
			settings.Model = Model;
			settings.SerialNumber = SerialNumber;
			settings.PurchaseDate = PurchaseDate;
			NetworkInfo.CopySettings(settings.NetworkInfo);
		}


		/// <summary>
		/// Clear the items from settings
		/// </summary>
		public void ClearSettings()
		{
			Make = null;
			Model = null;
			SerialNumber = null;
			PurchaseDate = null;

			NetworkInfo.ClearSettings();
		}

		#endregion

		#region Console

		/// <summary>
		/// Gets the name of the node.
		/// </summary>
		public override string ConsoleName { get { return "ConfiguredDeviceInfo"; } }

		/// <summary>
		/// Gets the help information for the node.
		/// </summary>
		public override string ConsoleHelp { get { return "Configured information for the device"; } }

		/// <summary>
		/// Calls the delegate for each console status item.
		/// </summary>
		/// <param name="addRow"></param>
		public override void BuildConsoleStatus(AddStatusRowDelegate addRow)
		{
			base.BuildConsoleStatus(addRow);

			addRow("Purchase Date", PurchaseDate.ToString());
		}

		#endregion
	}
}