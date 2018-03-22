using System.Collections.Generic;
using System.Linq;
using ICD.Connect.API.Nodes;

namespace ICD.Connect.Devices.Controls
{
	public sealed class ApiControlsNodeGroup : AbstractApiNodeGroup
	{
		private readonly DeviceControlsCollection m_Controls;

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="controls"></param>
		public ApiControlsNodeGroup(DeviceControlsCollection controls)
		{
			m_Controls = controls;
		}

		public override object this[uint key] { get { return m_Controls.GetControl((int)key); } }

		public override bool ContainsKey(uint key)
		{
			return m_Controls.Contains((int)key);
		}

		protected override IEnumerable<KeyValuePair<uint, object>> GetNodes()
		{
			return m_Controls.Select(c => new KeyValuePair<uint, object>((uint)c.Id, c));
		}
	}
}
