using System;
using System.Linq;
using System.Text.RegularExpressions;
using ICD.Common.Properties;
using ICD.Common.Utils;
using ICD.Common.Utils.Json;
using Newtonsoft.Json;

namespace ICD.Connect.Devices.Telemetry.DeviceInfo
{
	[JsonConverter(typeof(ToStringJsonConverter))]
	public sealed class IcdPhysicalAddress : IEquatable<IcdPhysicalAddress>, ICloneable
	{
		[NotNull]
		private readonly byte[] m_Bytes;

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="bytes"></param>
		public IcdPhysicalAddress([NotNull] byte[] bytes)
		{
			if (bytes == null)
				throw new ArgumentNullException("bytes");

			if (bytes.Length == 0)
				throw new ArgumentException("Byte array is empty");

			m_Bytes = bytes.ToArray();
		}

		/// <summary>
		/// Gets the string representation for the object.
		/// </summary>
		/// <returns></returns>
		public override string ToString()
		{
			return BitConverter.ToString(m_Bytes);
		}

		/// <summary>
		/// Gets the address bytes.
		/// </summary>
		/// <returns></returns>
		public byte[] GetAddressBytes()
		{
			return m_Bytes.ToArray();
		}

		/// <summary>
		/// Parses the given physical address string.
		/// </summary>
		/// <param name="address"></param>
		/// <returns></returns>
		[NotNull]
		public static IcdPhysicalAddress Parse([NotNull] string address)
		{
			IcdPhysicalAddress output;
			if (TryParse(address, out output))
				return output;

			throw new FormatException(string.Format("An invalid physical address was specified: '{0}'", address));
		}

		/// <summary>
		/// Attempts to parse the given physical address string.
		/// </summary>
		/// <param name="address"></param>
		/// <param name="physicalAddress"></param>
		/// <returns></returns>
		public static bool TryParse(string address, out IcdPhysicalAddress physicalAddress)
		{
			physicalAddress = null;

			if (address == null)
				return false;

			const string pattern = @"^((?:[0-9a-fA-F]{2}[\-\.;:]?)*(?:[0-9a-fA-F]{2}))$$";
			if (!Regex.IsMatch(address, pattern))
				return false;

			address = Regex.Replace(address, @"[\-\.;:]", string.Empty);
			byte[] bytes = StringUtils.HexToBytes(address);

			physicalAddress = new IcdPhysicalAddress(bytes);
			return true;
		}

		/// <summary>
		/// Returns true if the physical addresses are equal.
		/// </summary>
		/// <param name="other"></param>
		/// <returns></returns>
		public bool Equals(IcdPhysicalAddress other)
		{
			return other != null && m_Bytes.SequenceEqual(other.m_Bytes);
		}

		/// <summary>
		/// Gets the hash code for the physical address.
		/// </summary>
		/// <returns></returns>
		public override int GetHashCode()
		{
			unchecked
			{
				int hash = 17;

				for (int index = 0; index < m_Bytes.Length; index++)
					hash = hash * 23 + m_Bytes[index];

				return hash;
			}
		}

		/// <summary>
		/// Creates a new object that is a copy of the current instance.
		/// </summary>
		/// <returns></returns>
		object ICloneable.Clone()
		{
			return Clone();
		}

		/// <summary>
		/// Creates a new object that is a copy of the current instance.
		/// </summary>
		/// <returns></returns>
		public IcdPhysicalAddress Clone()
		{
			return new IcdPhysicalAddress(m_Bytes);
		}
	}
}
