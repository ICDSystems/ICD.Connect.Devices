using ICD.Common.Utils.EventArguments;
using ICD.Connect.Devices.Controls;

namespace ICD.Connect.Devices.EventArguments
{
	public sealed class DeviceControlEventArgs : GenericEventArgs<IDeviceControl>
	{
		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="data"></param>
		public DeviceControlEventArgs(IDeviceControl data)
			: base(data)
		{
		}
	}
}
