using System;
using System.Collections.Generic;
using ICD.Connect.API.Commands;
using ICD.Connect.API.Nodes;

namespace ICD.Connect.Devices
{
	public static class DeviceBaseConsole
	{
		/// <summary>
		/// Gets the child console nodes.
		/// </summary>
		/// <param name="instance"></param>
		/// <returns></returns>
		public static IEnumerable<IConsoleNodeBase> GetConsoleNodes(IDeviceBase instance)
		{
			if (instance == null)
				throw new ArgumentNullException("instance");

			yield return ConsoleNodeGroup.KeyNodeMap("Controls", instance.Controls, c => (uint)c.Id);
		}

		/// <summary>
		/// Calls the delegate for each console status item.
		/// </summary>
		/// <param name="instance"></param>
		/// <param name="addRow"></param>
		public static void BuildConsoleStatus(IDeviceBase instance, AddStatusRowDelegate addRow)
		{
			if (instance == null)
				throw new ArgumentNullException("instance");

			addRow("ID", instance.Id);
			addRow("Name", instance.Name);
			addRow("Online", instance.IsOnline);
		}

		/// <summary>
		/// Gets the child console commands.
		/// </summary>
		/// <param name="instance"></param>
		/// <returns></returns>
		public static IEnumerable<IConsoleCommand> GetConsoleCommands(IDeviceBase instance)
		{
			if (instance == null)
				throw new ArgumentNullException("instance");

			yield break;
		}
	}
}
