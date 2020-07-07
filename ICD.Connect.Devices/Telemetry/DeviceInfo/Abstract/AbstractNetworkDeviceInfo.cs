using System;
using System.Collections.Generic;
using ICD.Common.Utils.EventArguments;
using ICD.Common.Utils.Extensions;

namespace ICD.Connect.Devices.Telemetry.DeviceInfo.Abstract
{
	public abstract class AbstractNetworkDeviceInfo<TAdapterInfo> : INetworkDeviceInfo<TAdapterInfo>
		where TAdapterInfo : IAdapterNetworkDeviceInfo
	{

		#region Fields

		private string m_Hostname;
		private string m_Dns;
		private readonly Dictionary<int, TAdapterInfo> m_Adapters;

		#endregion

		#region Events

		public event EventHandler<StringEventArgs> OnHostnameChanged;
		public event EventHandler<StringEventArgs> OnDnsChanged;
		public event EventHandler OnAdaptersChanged;

		#endregion

		#region Properties

		public string Hostname
		{
			get { return m_Hostname; }
			set
			{
				if (value == m_Hostname)
					return;

				m_Hostname = value;

				OnHostnameChanged.Raise(this, new StringEventArgs(value));
			}
		}

		public string Dns
		{
			get { return m_Dns; }
			set
			{
				if (value == m_Dns)
					return;

				m_Dns = value;

				OnDnsChanged.Raise(this, new StringEventArgs(value));
			}
		}

		public string NodeIdentifier { get { return "0"; } }

		public IEnumerable<TAdapterInfo> Adapters { get { return m_Adapters.Values.ToList(m_Adapters.Count); } }

		#endregion

		protected abstract TAdapterInfo CreateNewAdapter(int address);

		public TAdapterInfo GetOrAddAdapter(int address)
		{
			TAdapterInfo adapter;
			if (!m_Adapters.TryGetValue(address, out adapter))
			{
				adapter = CreateNewAdapter(address);
				m_Adapters.Add(address, adapter);
				OnAdaptersChanged.Raise(this);
			}
			return adapter;
		}

		public virtual void Clear()
		{
			Hostname = null;
			Dns = null;
			m_Adapters.Clear();
		}

		public AbstractNetworkDeviceInfo()
		{
			m_Adapters = new Dictionary<int, TAdapterInfo>();
		}

		protected void AdaptersClear()
		{
			m_Adapters.Clear();
		}

		protected void AdaptersAddRange(IEnumerable<TAdapterInfo> adapters)
		{
			m_Adapters.AddRange(adapters, a => a.Address);
		}

	}
}