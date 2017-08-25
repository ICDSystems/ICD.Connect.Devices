using System;
using System.Collections.Generic;
using System.Linq;
using ICD.Common.Properties;
using ICD.Common.Utils;
using ICD.Common.Utils.Extensions;
using ICD.Connect.API.Nodes;

namespace ICD.Connect.Devices.Controls
{
	/// <summary>
	/// IDeviceControl contains the usage features for a parent device.
	/// </summary>
	public interface IDeviceControl : IConsoleNode, IStateDisposable
	{
		/// <summary>
		/// Gets the parent device for this control.
		/// </summary>
		IDevice Parent { get; }

		/// <summary>
		/// Gets the id for this control.
		/// </summary>
		int Id { get; }

		/// <summary>
		/// Gets the human readable name for this control.
		/// </summary>
		[PublicAPI]
		string Name { get; }

		/// <summary>
		/// Gets the parent and control id info.
		/// </summary>
		DeviceControlInfo DeviceControlInfo { get; }
	}

	public static class DeviceControlExtensions
	{
		/// <summary>
		/// Gets the sibling control with the given id.
		/// </summary>
		/// <param name="extends"></param>
		/// <param name="id"></param>
		/// <returns></returns>
		[NotNull]
		[PublicAPI]
		public static IDeviceControl GetSibling(this IDeviceControl extends, int id)
		{
			if (extends == null)
				throw new ArgumentNullException("extends");

			return extends.Parent
			              .Controls
			              .GetControl(id);
		}

		/// <summary>
		/// Gets the first sibling control of the given type.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="extends"></param>
		/// <returns></returns>
		[NotNull]
		[PublicAPI]
		public static T GetSibling<T>(this IDeviceControl extends)
			where T : IDeviceControl
		{
			if (extends == null)
				throw new ArgumentNullException("extends");

			return extends.GetSiblings()
			              .Except(extends)
			              .OfType<T>()
			              .First();
		}

		/// <summary>
		/// Gets the sibling control with the given id and type.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="extends"></param>
		/// <param name="id"></param>
		/// <returns></returns>
		[NotNull]
		[PublicAPI]
		public static T GetSibling<T>(this IDeviceControl extends, int id)
			where T : IDeviceControl
		{
			if (extends == null)
				throw new ArgumentNullException("extends");

			return extends.Parent
			              .Controls
			              .GetControl<T>(id);
		}

		/// <summary>
		/// Gets all of the controls on the parent device except this control.
		/// </summary>
		/// <param name="extends"></param>
		/// <returns></returns>
		[NotNull]
		[PublicAPI]
		public static IEnumerable<IDeviceControl> GetSiblings(this IDeviceControl extends)
		{
			if (extends == null)
				throw new ArgumentNullException("extends");

			return extends.Parent
			              .Controls
			              .Except(extends);
		}

		/// <summary>
		/// Gets all of the controls of the given type on the parent device except this control.
		/// </summary>
		/// <param name="extends"></param>
		/// <returns></returns>
		[NotNull]
		[PublicAPI]
		public static IEnumerable<T> GetSiblings<T>(this IDeviceControl extends)
		{
			if (extends == null)
				throw new ArgumentNullException("extends");

			return extends.GetSiblings()
			              .OfType<T>();
		}
	}
}
