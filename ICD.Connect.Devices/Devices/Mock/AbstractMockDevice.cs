using System.Collections.Generic;
using ICD.Connect.API.Commands;
using ICD.Connect.API.Nodes;
using ICD.Connect.Settings;

namespace ICD.Connect.Devices.Mock
{
	public abstract class AbstractMockDevice<TSettings> : AbstractDevice<TSettings>, IMockDevice where TSettings : IMockDeviceSettings, new()
	{

		private bool m_IsOnline;

		public bool DefaultOffline { get; set; }

		/// <summary>
		/// Gets the current online status of the device.
		/// </summary>
		/// <returns></returns>
		protected override bool GetIsOnlineStatus()
		{
			return m_IsOnline;
		}

		public void SetIsOnlineState(bool isOnline)
		{
			m_IsOnline = isOnline;
			UpdateCachedOnlineStatus();
		}

		#region Settings

		/// <summary>
		/// Override to apply settings to the instance.
		/// </summary>
		/// <param name="settings"></param>
		/// <param name="factory"></param>
		protected override void ApplySettingsFinal(TSettings settings, IDeviceFactory factory)
		{
			base.ApplySettingsFinal(settings, factory);

			MockDeviceHelper.ApplySettings(this, settings);
		}

		/// <summary>
		/// Override to apply properties to the settings instance.
		/// </summary>
		/// <param name="settings"></param>
		protected override void CopySettingsFinal(TSettings settings)
		{
			base.CopySettingsFinal(settings);

			MockDeviceHelper.CopySettings(this, settings);
		}

		/// <summary>
		/// Override to clear the instance settings.
		/// </summary>
		protected override void ClearSettingsFinal()
		{
			base.ClearSettingsFinal();

			MockDeviceHelper.ClearSettings(this);
		}

		#endregion

		#region Console

		/// <summary>
		/// Gets the child console commands.
		/// </summary>
		/// <returns></returns>
		public override IEnumerable<IConsoleCommand> GetConsoleCommands()
		{
			foreach (IConsoleCommand command in GetBaseConsoleCommands())
				yield return command;

			foreach (IConsoleCommand command in MockDeviceHelper.GetConsoleCommands(this))
				yield return command;
		}

		private IEnumerable<IConsoleCommand> GetBaseConsoleCommands()
		{
			return base.GetConsoleCommands();
		}

		/// <summary>
		/// Calls the delegate for each console status item.
		/// </summary>
		/// <param name="addRow"></param>
		public override void BuildConsoleStatus(AddStatusRowDelegate addRow)
		{
			base.BuildConsoleStatus(addRow);

			MockDeviceHelper.BuildConsoleStatus(this, addRow);
		}

		#endregion
	}
}