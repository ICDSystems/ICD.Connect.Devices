using ICD.Common.Utils.Extensions;
using ICD.Connect.Telemetry.Attributes;
using ICD.Connect.Telemetry.Providers.External;

namespace ICD.Connect.Devices.Controls
{
	public sealed class DeviceControlExternalTelemetryProvider : AbstractExternalTelemetryProvider<IDeviceControl>
	{
		[PropertyTelemetry("DeviceControlType", null, null)]
		public string DeviceControlType { get { return Parent.GetType().GetMinimalName(); } }
	}
}
