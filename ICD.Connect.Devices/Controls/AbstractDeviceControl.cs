﻿using System;
using System.Collections.Generic;
using ICD.Common.Logging.Activities;
using ICD.Common.Logging.LoggingContexts;
using ICD.Common.Properties;
using ICD.Common.Utils;
using ICD.Common.Utils.Extensions;
using ICD.Connect.API.Commands;
using ICD.Connect.API.Nodes;
using ICD.Connect.Devices.EventArguments;
using ICD.Connect.Devices.Utils;

namespace ICD.Connect.Devices.Controls
{
	/// <summary>
	/// Base class for device controls that wrap a specific device type.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public abstract class AbstractDeviceControl<T> : IDeviceControl<T>
		where T : IDevice
	{
		#region Events

		/// <summary>
		/// Raised when the Control availability changes
		/// </summary>
		public event EventHandler<DeviceControlAvailableApiEventArgs> OnControlAvailableChanged;

		#endregion

		#region Fields

		private readonly int m_Id;
		private readonly Guid m_Uuid;
		private readonly T m_Parent;
		private readonly ILoggingContext m_Logger;
		private readonly IActivityContext m_Activities;

		private bool m_ControlAvailable;

		#endregion

		#region Properties

		/// <summary>
		/// Gets the parent device for this control.
		/// </summary>
		IDevice IDeviceControl.Parent { get { return Parent; } }

		/// <summary>
		/// Gets the id for this control.
		/// </summary>
		public int Id { get { return m_Id; } }

		/// <summary>
		/// Unique ID for the control.
		/// </summary>
		public Guid Uuid { get { return m_Uuid; } }

		/// <summary>
		/// Gets the parent device for this control.
		/// </summary>
		[PublicAPI]
		public T Parent { get { return m_Parent; } }

		/// <summary>
		/// Gets the human readable name for this control.
		/// </summary>
		public virtual string Name { get { return GetType().GetNameWithoutGenericArity(); } }

		/// <summary>
		/// Gets if the control is currently available or not
		/// </summary>
		public bool ControlAvailable
		{
			get { return m_ControlAvailable; }
			private set
			{
				if (value == m_ControlAvailable)
					return;

				m_ControlAvailable = value;

				OnControlAvailableChanged.Raise(this, new DeviceControlAvailableApiEventArgs(ControlAvailable));
			}
		}

		/// <summary>
		/// Gets the name of the node.
		/// </summary>
		public virtual string ConsoleName { get { return string.IsNullOrEmpty(Name) ? GetType().GetNameWithoutGenericArity() : Name; } }

		/// <summary>
		/// Gets the help information for the node.
		/// </summary>
		public virtual string ConsoleHelp { get { return string.Empty; } }

		/// <summary>
		/// Returns true if this instance has been disposed.
		/// </summary>
		public bool IsDisposed { get; private set; }

		/// <summary>
		/// Gets the logger for the control.
		/// </summary>
		public ILoggingContext Logger { get { return m_Logger; } }

		/// <summary>
		/// Gets the activities for this instance.
		/// </summary>
		public IActivityContext Activities { get { return m_Activities; } }

		#endregion

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="parent"></param>
		/// <param name="id"></param>
		protected AbstractDeviceControl([NotNull] T parent, int id)
			: this(parent, id, DeviceControlUtils.GenerateUuid(parent, id))
		{
		}

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="parent"></param>
		/// <param name="id"></param>
		/// <param name="uuid"></param>
		protected AbstractDeviceControl([NotNull] T parent, int id, Guid uuid)
		{
			if (parent == null)
				throw new ArgumentNullException("parent");

			m_Id = id;
			m_Uuid = uuid;
			m_Parent = parent;
			m_Logger = new ServiceLoggingContext(this);
			m_Activities = new ActivityContext();

			Subscribe(Parent);
			UpdateCachedControlAvailable();
		}

		/// <summary>
		/// Destructor.
		/// </summary>
		~AbstractDeviceControl()
		{
			Dispose(false);
		}

		#region Methods

		/// <summary>
		/// Release resources.
		/// </summary>
		public void Dispose()
		{
			Dispose(true);
		}

		/// <summary>
		/// Gets the string representation for this instance.
		/// </summary>
		/// <returns></returns>
		public override string ToString()
		{
			ReprBuilder builder = new ReprBuilder(this);

			builder.AppendProperty("Id", Id);
			builder.AppendProperty("Parent", Parent.Id);

			return builder.ToString();
		}

		/// <summary>
		/// Initializes the current telemetry state.
		/// </summary>
		public virtual void InitializeTelemetry()
		{
		}

		protected virtual bool GetControlAvailable()
		{
			return Parent.ControlsAvailable;
		}

		protected virtual void UpdateCachedControlAvailable()
		{
			ControlAvailable = GetControlAvailable();
		}

		#endregion

		#region Private Methods

		/// <summary>
		/// Releases resources but also allows for finalizing without touching managed resources.
		/// </summary>
		/// <param name="disposing"></param>
		private void Dispose(bool disposing)
		{
			if (!IsDisposed)
				DisposeFinal(disposing);
			IsDisposed = IsDisposed || disposing;
		}

		/// <summary>
		/// Override to release resources.
		/// </summary>
		/// <param name="disposing"></param>
		protected virtual void DisposeFinal(bool disposing)
		{
			Unsubscribe(Parent);
		}

		#endregion

		#region Parent Callbacks

		/// <summary>
		/// Subscribe to the parent events.
		/// </summary>
		/// <param name="parent"></param>
		protected virtual void Subscribe(T parent)
		{
			if (parent == null)
				return;

			parent.OnControlsAvailableChanged += ParentOnControlsAvailableChanged;
		}

		/// <summary>
		/// Unsubscribe from the parent events.
		/// </summary>
		/// <param name="parent"></param>
		protected virtual void Unsubscribe(T parent)
		{
			if (parent == null)
				return;

			parent.OnControlsAvailableChanged -= ParentOnControlsAvailableChanged;
		}

		/// <summary>
		/// Called when the parent device control availability changes.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void ParentOnControlsAvailableChanged(object sender, DeviceBaseControlsAvailableApiEventArgs e)
		{
			UpdateCachedControlAvailable();
		}

		#endregion

		#region Console

		/// <summary>
		/// Gets the child console nodes.
		/// </summary>
		/// <returns></returns>
		public virtual IEnumerable<IConsoleNodeBase> GetConsoleNodes()
		{
			return DeviceControlConsole.GetConsoleNodes(this);
		}

		/// <summary>
		/// Calls the delegate for each console status item.
		/// </summary>
		/// <param name="addRow"></param>
		public virtual void BuildConsoleStatus(AddStatusRowDelegate addRow)
		{
			DeviceControlConsole.BuildConsoleStatus(this, addRow);
		}

		/// <summary>
		/// Gets the child console commands.
		/// </summary>
		/// <returns></returns>
		public virtual IEnumerable<IConsoleCommand> GetConsoleCommands()
		{
			return DeviceControlConsole.GetConsoleCommands(this);
		}

		#endregion
	}
}
