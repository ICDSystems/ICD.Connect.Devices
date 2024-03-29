﻿using ICD.Common.Properties;
using ICD.Connect.Settings;

namespace ICD.Connect.Devices.Extensions
{
	public static class DeviceFactoryExtensions
	{
		/// <summary>
		/// Lazy-loads the device with the given id.
		/// </summary>
		/// <param name="factory"></param>
		/// <param name="id"></param>
		/// <returns></returns>
		[PublicAPI]
		[NotNull]
		public static IDevice GetDeviceById(this IDeviceFactory factory, int id)
		{
			return factory.GetOriginatorById<IDevice>(id);
		}
	}
}
