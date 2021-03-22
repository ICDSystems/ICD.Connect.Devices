using System.Linq;
using ICD.Connect.Devices.Telemetry.DeviceInfo;
using NUnit.Framework;

namespace ICD.Connect.Devices.Tests.Devices.Telemetry.DeviceInfo
{
	[TestFixture]
	public sealed class IcdPhysicalAddressTest
	{
		[TestCase("12", new byte[] {0x12})]
		[TestCase("12-AB-CD-34", new byte[] {0x12, 0xAB, 0xCD, 0x34})]
		public void ToStringTest(string expected, byte[] bytes)
		{
			IcdPhysicalAddress address = new IcdPhysicalAddress(bytes);
			Assert.AreEqual(expected, address.ToString());
		}

		[TestCase("12", true, new byte[] {0x12})]
		[TestCase("12-AB-CD-34", true, new byte[] {0x12, 0xAB, 0xCD, 0x34})]
		[TestCase(null, false, new byte[] { })]
		[TestCase("", false, new byte[] { })]
		[TestCase("1", false, new byte[] { })]
		[TestCase("12-3", false, new byte[] { })]
		public void TryParseTest(string data, bool expected, byte[] expectedBytes)
		{
			IcdPhysicalAddress address;
			bool actual = IcdPhysicalAddress.TryParse(data, out address);

			byte[] actualBytes = address == null ? new byte[0] : address.GetAddressBytes();

			Assert.AreEqual(expected, actual);
			Assert.IsTrue(expectedBytes.SequenceEqual(actualBytes));
		}
	}
}
