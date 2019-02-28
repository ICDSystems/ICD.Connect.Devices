using System;
using ICD.Common.Properties;
using ICD.Common.Utils;
using ICD.Common.Utils.Xml;

namespace ICD.Connect.Devices.Controls
{
	/// <summary>
	/// Simple pairing of device and control ids.
	/// </summary>
	public struct DeviceControlInfo : IComparable<DeviceControlInfo>, IEquatable<DeviceControlInfo>
	{
		private const string DEVICE_ELEMENT = "Device";
		private const string CONTROL_ELEMENT = "Control";

		private readonly int m_DeviceId;
		private readonly int m_ControlId;

		/// <summary>
		/// Gets the device id.
		/// </summary>
		public int DeviceId { get { return m_DeviceId; } }

		/// <summary>
		/// Gets the control id.
		/// </summary>
		public int ControlId { get { return m_ControlId; } }

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="deviceId"></param>
		/// <param name="controlId"></param>
		public DeviceControlInfo(int deviceId, int controlId)
		{
			m_DeviceId = deviceId;
			m_ControlId = controlId;
		}

		public override string ToString()
		{
			ReprBuilder builder = new ReprBuilder(this);

			builder.AppendProperty("DeviceId", DeviceId);
			builder.AppendProperty("ControlId", ControlId);

			return builder.ToString();
		}

		/// <summary>
		/// Writes the device control info to the xml writer.
		/// </summary>
		/// <param name="writer"></param>
		/// <param name="element"></param>
		public void WriteToXml(IcdXmlTextWriter writer, string element)
		{
			writer.WriteStartElement(element);
			{
				writer.WriteElementString(DEVICE_ELEMENT, IcdXmlConvert.ToString(DeviceId));
				writer.WriteElementString(CONTROL_ELEMENT, IcdXmlConvert.ToString(ControlId));
			}
			writer.WriteEndElement();
		}

		/// <summary>
		/// Deserializes the device control info from an xml element.
		/// </summary>
		/// <param name="xml"></param>
		/// <returns></returns>
		public static DeviceControlInfo ReadFromXml(string xml)
		{
			int deviceId = XmlUtils.TryReadChildElementContentAsInt(xml, DEVICE_ELEMENT) ?? 0;
			int controlId = XmlUtils.TryReadChildElementContentAsInt(xml, CONTROL_ELEMENT) ?? 0;

			return new DeviceControlInfo(deviceId, controlId);
		}

		#region Equality

		/// <summary>
		/// Implementing default equality.
		/// </summary>
		/// <param name="a1"></param>
		/// <param name="a2"></param>
		/// <returns></returns>
		public static bool operator ==(DeviceControlInfo a1, DeviceControlInfo a2)
		{
			return a1.Equals(a2);
		}

		/// <summary>
		/// Implementing default inequality.
		/// </summary>
		/// <param name="a1"></param>
		/// <param name="a2"></param>
		/// <returns></returns>
		public static bool operator !=(DeviceControlInfo a1, DeviceControlInfo a2)
		{
			return !a1.Equals(a2);
		}

		[Pure]
		public int CompareTo(DeviceControlInfo other)
		{
// ReSharper disable ImpureMethodCallOnReadonlyValueField
			int result = m_DeviceId.CompareTo(other.m_DeviceId);
			if (result != 0)
				return result;

			return m_ControlId.CompareTo(other.m_ControlId);
// ReSharper restore ImpureMethodCallOnReadonlyValueField
		}

		public bool Equals(DeviceControlInfo other)
		{
			return m_DeviceId == other.m_DeviceId &&
			       m_ControlId == other.m_ControlId;
		}

		/// <summary>
		/// Returns true if this instance is equal to the given object.
		/// </summary>
		/// <param name="other"></param>
		/// <returns></returns>
		public override bool Equals(object other)
		{
			return other is DeviceControlInfo && Equals((DeviceControlInfo)other);
		}

		/// <summary>
		/// Gets the hashcode for this instance.
		/// </summary>
		/// <returns></returns>
		[Pure]
		public override int GetHashCode()
		{
			unchecked
			{
				int hash = 17;
				hash = hash * 23 + m_DeviceId;
				hash = hash * 23 + m_ControlId;
				return hash;
			}
		}

		#endregion
	}
}
