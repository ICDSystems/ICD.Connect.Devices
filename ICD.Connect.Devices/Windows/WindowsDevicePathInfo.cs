using System;
using System.Text.RegularExpressions;
using ICD.Common.Properties;
using ICD.Common.Utils.Extensions;

namespace ICD.Connect.Devices.Windows
{
	public struct WindowsDevicePathInfo : IEquatable<WindowsDevicePathInfo>, IComparable<WindowsDevicePathInfo>
	{
		/// <summary>
		/// Finds a tuple in the format "TYPE\DEVICE_ID\INSTANCE_ID".
		/// </summary>
		private const string PARSE_REGEX = @"(?'type'\w+)\\(?'deviceId'[\w&]+)\\(?'instanceId'[\w&]+)";

		private const string DEVICE_ID_REGEX = @"(?'VendorId'VID[\w&]{6})(?'productId'PID[\w&]{6})(?'InterfaceId'MI[\w]{3}|REV[\w]{5})";

		/// <summary>
		/// Characters that are illegal for valid device instances.
		/// </summary>
		private const string INVALID_CHARACTERS_REGEX = @"[^\w&\\]";

		public const string DEFAULT_TYPE = "000";
		public const string DEFAULT_DEVICE_ID = "VID_0000&PID_0000&MI_00";
		public const string DEFAULT_INSTANCE_ID = "0&0000000&0&0000";

		[CanBeNull]
		private readonly string m_Type;

		[CanBeNull]
		private readonly string m_DeviceId;

		[CanBeNull]
		private readonly string m_InstanceId;

		#region Properties

		/// <summary>
		/// Gets the type of device, e.g. USB, PCI, etc.
		/// </summary>
		[NotNull]
		public string Type { get { return m_Type ?? DEFAULT_TYPE; } }

		/// <summary>
		/// Gets the identifier for the device.
		/// </summary>
		[NotNull]
		public string DeviceId { get { return m_DeviceId ?? DEFAULT_DEVICE_ID; } }

		/// <summary>
		/// Gets the instance identifier for the device.
		/// </summary>
		[NotNull]
		public string InstanceId { get { return m_InstanceId ?? DEFAULT_INSTANCE_ID; } }

		#endregion

		#region Constructors

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="type"></param>
		/// <param name="deviceId"></param>
		/// <param name="instanceId"></param>
		public WindowsDevicePathInfo([CanBeNull] string type, [CanBeNull] string deviceId, [CanBeNull] string instanceId)
		{
			m_Type = Sanitize(type);
			m_DeviceId = Sanitize(deviceId);
			m_InstanceId = Sanitize(instanceId);
		}

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="devicePath"></param>
		public WindowsDevicePathInfo([NotNull] string devicePath)
		{
			WindowsDevicePathInfo pathInfo = Parse(devicePath);

			m_Type = pathInfo.Type;
			m_DeviceId = pathInfo.DeviceId;
			m_InstanceId = pathInfo.InstanceId;
		}

		public static WindowsDevicePathInfo Parse(string data)
		{
			WindowsDevicePathInfo pathInfo;
			if (TryParse(data, out pathInfo))
				return pathInfo;

			throw new FormatException("Unexpected device instance format");
		}

		public static bool TryParse(string data, out WindowsDevicePathInfo pathInfo)
		{
			pathInfo = default(WindowsDevicePathInfo);

			// Zoom - USB Huddly GO Camera
			// "\\\\?\\usb#vid_2bd9&pid_0011&mi_00#6&a1e91ba&0&0000#{65e8773d-8f56-11d0-a3b9-00a0c9223196}\\global"

			// Windows - USB Huddly GO Camera
			// USB\VID_2BD9&PID_0011&MI_00\6&A1E91BA&0&0000

			// PCI example
			// PCI\VEN_1000&DEV_0001&SUBSYS_00000000&REV_02\1&08

			// Replace non alphanumeric, ampersand, or backslash characters with a backslash
			data = Sanitize(data) ?? string.Empty;

			// Remove leading or trailing nonsense
			Match match = Regex.Match(data, PARSE_REGEX);
			if (!match.Success)
				return false;

			string type = match.Groups["type"].Value;
			string deviceId = match.Groups["deviceId"].Value;
			string instanceId = match.Groups["instanceId"].Value;

			pathInfo = new WindowsDevicePathInfo(type, deviceId, instanceId);
			return true;
		}

		#endregion

		#region Methods

		/// <summary>
		/// Gets the string representation for this instance.
		/// </summary>
		/// <returns></returns>
		public override string ToString()
		{
			return string.Format("{0}\\{1}\\{2}", Type, DeviceId, InstanceId);
		}

		/// <summary>
		/// Massages the data to replace illegal characters.
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		[CanBeNull]
		private static string Sanitize([CanBeNull] string value)
		{
			if (value == null)
				return null;

			// Replace illegal characters with a backslash (some ids )
			value = Regex.Replace(value, INVALID_CHARACTERS_REGEX, @"\");

			// Use uppercase
			return value.ToUpper();
		}

		public static WindowsDevicePathInfo SetDeviceId(WindowsDevicePathInfo pathInfo, string deviceId)
		{
			if (!Regex.IsMatch(deviceId, DEVICE_ID_REGEX))
				throw new FormatException("Invalid Device ID");

			return new WindowsDevicePathInfo(pathInfo.Type, deviceId, pathInfo.InstanceId);
		}

		#endregion

		#region Equality

		/// <summary>
		/// Implementing default equality.
		/// </summary>
		/// <param name="a1"></param>
		/// <param name="a2"></param>
		/// <returns></returns>
		public static bool operator ==(WindowsDevicePathInfo a1, WindowsDevicePathInfo a2)
		{
			return a1.Equals(a2);
		}

		/// <summary>
		/// Implementing default inequality.
		/// </summary>
		/// <param name="a1"></param>
		/// <param name="a2"></param>
		/// <returns></returns>
		public static bool operator !=(WindowsDevicePathInfo a1, WindowsDevicePathInfo a2)
		{
			return !a1.Equals(a2);
		}

		/// <summary>
		/// Returns true if this instance is equal to the given object.
		/// </summary>
		/// <param name="other"></param>
		/// <returns></returns>
		public override bool Equals(object other)
		{
			return other is WindowsDevicePathInfo && Equals((WindowsDevicePathInfo)other);
		}

		/// <summary>
		/// Returns true if this instance is equal to the given endpoint.
		/// </summary>
		/// <param name="other"></param>
		/// <returns></returns>
		[Pure]
		public bool Equals(WindowsDevicePathInfo other)
		{
			return string.Equals(Type, other.Type, StringComparison.OrdinalIgnoreCase) &&
			       string.Equals(DeviceId, other.DeviceId, StringComparison.OrdinalIgnoreCase) &&
			       string.Equals(InstanceId, other.InstanceId, StringComparison.OrdinalIgnoreCase);
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
				hash = hash * 23 + Type.ToUpper().GetStableHashCode();
				hash = hash * 23 + DeviceId.ToUpper().GetStableHashCode();
				hash = hash * 23 + InstanceId.ToUpper().GetStableHashCode();
				return hash;
			}
		}

		public int CompareTo(WindowsDevicePathInfo other)
		{
			int result = string.CompareOrdinal(Type.ToUpper(), other.Type.ToUpper());
			if (result != 0)
				return result;

			result = string.CompareOrdinal(DeviceId.ToUpper(), other.DeviceId.ToUpper());
			if (result != 0)
				return result;

			return string.CompareOrdinal(InstanceId.ToUpper(), other.InstanceId.ToUpper());
		}


		#endregion
	}
}