using System;
using System.Collections.Generic;
using ICD.Connect.API.Commands;
using ICD.Connect.API.Nodes;

namespace ICD.Connect.Devices.Controls
{
	public static class PowerDeviceControlConsole
	{
		/// <summary>
		/// Gets the child console nodes.
		/// </summary>
		/// <param name="instance"></param>
		/// <returns></returns>
		public static IEnumerable<IConsoleNodeBase> GetConsoleNodes(IPowerDeviceControl instance)
		{
			if (instance == null)
				throw new ArgumentNullException("instance");

			yield break;
		}

		/// <summary>
		/// Calls the delegate for each console status item.
		/// </summary>
		/// <param name="instance"></param>
		/// <param name="addRow"></param>
		public static void BuildConsoleStatus(IPowerDeviceControl instance, AddStatusRowDelegate addRow)
		{
			if (instance == null)
				throw new ArgumentNullException("instance");

			addRow("PowerState", instance.PowerState);
		}

		/// <summary>
		/// Gets the child console commands.
		/// </summary>
		/// <param name="instance"></param>
		/// <returns></returns>
		public static IEnumerable<IConsoleCommand> GetConsoleCommands(IPowerDeviceControl instance)
		{
			if (instance == null)
				throw new ArgumentNullException("instance");

			yield return new ConsoleCommand("PowerOn", "Turns on the device", () => instance.PowerOn());
			yield return new ConsoleCommand("PowerOff", "Turns off the device", () => instance.PowerOff());
		}
	}
}
