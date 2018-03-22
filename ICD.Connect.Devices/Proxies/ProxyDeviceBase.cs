using System;

namespace ICD.Connect.Devices.Proxies
{
    public sealed class ProxyDeviceBase : AbstractDeviceBase<ProxyDeviceBaseSettings>
    {
		protected override bool GetIsOnlineStatus()
		{
			return true;
		}
	}

	public sealed class ProxyDeviceBaseSettings : AbstractDeviceBaseSettings
	{
		protected override string Element { get { return null; } }

		public override string FactoryName { get { return null; } }

		public override Type OriginatorType { get { return null; } }
	}
}
