using System;
using System.Collections.Generic;
using System.Linq;
using ICD.Common.Properties;
using ICD.Common.Utils;
using ICD.Common.Utils.Collections;
using ICD.Common.Utils.Extensions;
using ICD.Connect.Devices.EventArguments;
using ICD.Connect.Telemetry.Attributes;
using ICD.Connect.Telemetry.Providers.External;

namespace ICD.Connect.Devices
{
	public sealed class DeviceExternalTelemetryProvider : AbstractExternalTelemetryProvider<IDevice>
	{
		[PublicAPI("DAV-PRO")]
		[EventTelemetry("OnControlIdsChanged")]
		public event EventHandler OnControlIdsChanged;

		private readonly IcdHashSet<Guid> m_ControlIds;
		private readonly SafeCriticalSection m_ControlIdsSection;

		#region Properties

		[PublicAPI("DAV-PRO")]
		[PropertyTelemetry("ControlIds", null, "OnControlIdsChanged")]
		public IEnumerable<Guid> ControlIds { get { return m_ControlIdsSection.Execute(() => m_ControlIds.ToArray()); } }
		
		#endregion

		/// <summary>
		/// Constructor.
		/// </summary>
		public DeviceExternalTelemetryProvider()
		{
			m_ControlIds = new IcdHashSet<Guid>();
			m_ControlIdsSection = new SafeCriticalSection();
		}

		#region Methods

		/// <summary>
		/// Sets the parent telemetry provider that this instance extends.
		/// </summary>
		/// <param name="parent"></param>
		protected override void SetParent(IDevice parent)
		{
			base.SetParent(parent);

			UpdateControlIds();
		}

		#endregion

		#region Private Methods

		/// <summary>
		/// Updates the ControlIds collection and raises the OnControlIdsChanged event
		/// if the collection has changed.
		/// </summary>
		private void UpdateControlIds()
		{
			IcdHashSet<Guid> controlIds =
				Parent.Controls
					  .Select(c => c.Uuid)
					  .ToIcdHashSet();

			m_ControlIdsSection.Enter();

			try
			{
				if (controlIds.SetEquals(m_ControlIds))
					return;

				m_ControlIds.Clear();
				m_ControlIds.AddRange(controlIds);
			}
			finally
			{
				m_ControlIdsSection.Leave();
			}

			OnControlIdsChanged.Raise(this);
		}

		#endregion

		#region Provider Callbacks

		/// <summary>
		/// Subscribe to the parent events.
		/// </summary>
		/// <param name="parent"></param>
		protected override void Subscribe(IDevice parent)
		{
			base.Subscribe(parent);

			if (parent == null)
				return;

			parent.Controls.OnControlAdded += ControlsOnControlAdded;
			parent.Controls.OnControlRemoved += ControlsOnControlRemoved;
		}

		/// <summary>
		/// Unsubscribe from the parent events.
		/// </summary>
		/// <param name="parent"></param>
		protected override void Unsubscribe(IDevice parent)
		{
			base.Unsubscribe(parent);

			if (parent == null)
				return;

			parent.Controls.OnControlAdded -= ControlsOnControlAdded;
			parent.Controls.OnControlRemoved -= ControlsOnControlRemoved;
		}

		/// <summary>
		/// Called when a control is added to the collection.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="deviceControlEventArgs"></param>
		private void ControlsOnControlAdded(object sender, DeviceControlEventArgs deviceControlEventArgs)
		{
			UpdateControlIds();
		}

		/// <summary>
		/// Called when a control is removed from the collection.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="deviceControlEventArgs"></param>
		private void ControlsOnControlRemoved(object sender, DeviceControlEventArgs deviceControlEventArgs)
		{
			UpdateControlIds();
		}

		#endregion
	}
}
