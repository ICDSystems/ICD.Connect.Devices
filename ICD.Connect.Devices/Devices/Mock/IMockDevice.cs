using System;
using System.Collections.Generic;
using ICD.Common.Properties;
using ICD.Connect.API.Commands;
using ICD.Connect.API.Nodes;

namespace ICD.Connect.Devices.Mock
{
	public interface IMockDevice : IDevice
	{
		bool DefaultOffline { get; set; }

		void SetIsOnlineState(bool isOnline);
	}

	public static class MockDeviceHelper
	{

		public static void BuildConsoleStatus([NotNull] IMockDevice device, [NotNull] AddStatusRowDelegate addRow)
		{
			if (device == null)
				throw new ArgumentNullException("device");
			
			if (addRow == null)
				throw new ArgumentNullException("addRow");

			addRow("Default Offline", device.DefaultOffline);
		}

		public static IEnumerable<IConsoleCommand> GetConsoleCommands([NotNull] IMockDevice device)
		{
			if (device == null)
				throw new ArgumentNullException("device");

			yield return new GenericConsoleCommand<bool>("SetOnline", "SetsOnline [true|false]", b => device.SetIsOnlineState(b));
		}

		public static void ApplySettings([NotNull] IMockDevice device, [NotNull] IMockDeviceSettings settings)
		{
			if (device == null)
				throw new ArgumentNullException("device");

			if (settings == null)
				throw new ArgumentNullException("settings");

			device.DefaultOffline = settings.DefaultOffline;

			device.SetIsOnlineState(!device.DefaultOffline);
		}

		public static void CopySettings([NotNull] IMockDevice device, [NotNull] IMockDeviceSettings settings)
		{
			if (device == null)
				throw new ArgumentNullException("device");

			if (settings == null)
				throw new ArgumentNullException("settings");

			settings.DefaultOffline = device.DefaultOffline;
		}

		/// <summary>
		/// Override to clear the instance settings.
		/// </summary>
		public static void ClearSettings([NotNull] IMockDevice device)
		{
			if (device == null)
				throw new ArgumentNullException("device");

			device.DefaultOffline = false;
		}
	}
}