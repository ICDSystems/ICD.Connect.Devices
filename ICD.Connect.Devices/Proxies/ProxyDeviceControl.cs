﻿namespace ICD.Connect.Devices.Proxies
{
	public sealed class ProxyDeviceControl : AbstractProxyDeviceControl
	{
		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="parent"></param>
		/// <param name="id"></param>
		public ProxyDeviceControl(IProxyDeviceBase parent, int id)
			: base(parent, id)
		{
		}
	}
}