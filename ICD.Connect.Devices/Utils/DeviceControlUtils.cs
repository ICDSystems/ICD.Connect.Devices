using System;
using ICD.Common.Properties;
using ICD.Common.Utils;

namespace ICD.Connect.Devices.Utils
{
	public static class DeviceControlUtils
	{
		/// <summary>
		/// Generates a UUID for the control based on the parent UUID and the given legacy ID.
		/// </summary>
		/// <param name="parent"></param>
		/// <param name="id"></param>
		/// <returns></returns>
		public static Guid GenerateUuid([NotNull] IDevice parent, int id)
		{
			if (parent == null)
				throw new ArgumentNullException("parent");

			Guid idSeed = GuidUtils.GenerateSeeded(id);
			return GuidUtils.Combine(parent.Uuid, idSeed);
		}
	}
}
