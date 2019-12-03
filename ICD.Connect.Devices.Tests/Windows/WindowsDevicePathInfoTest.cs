using System.Collections.Generic;
using System.Linq;
using ICD.Common.Utils.Extensions;
using ICD.Connect.Devices.Windows;
using NUnit.Framework;

namespace ICD.Connect.Devices.Tests.Windows
{
	[TestFixture]
	public sealed class WindowsDevicePathInfoTest
	{
		#region Properties

		[TestCase("test", "TEST")]
		[TestCase("TEST", "TEST")]
		[TestCase(null, null)]
		public void TypeTest(string type, string expected)
		{
			Assert.AreEqual(expected, new WindowsDevicePathInfo(type, null, null).Type);
		}

		[TestCase("test", "TEST")]
		[TestCase("TEST", "TEST")]
		[TestCase(null, null)]
		public void DeviceIdTest(string deviceId, string expected)
		{
			Assert.AreEqual(expected, new WindowsDevicePathInfo(null, deviceId, null).DeviceId);
		}

		[TestCase("test", "TEST")]
		[TestCase("TEST", "TEST")]
		[TestCase(null, null)]
		public void InstanceIdTest(string instanceId, string expected)
		{
			Assert.AreEqual(expected, new WindowsDevicePathInfo(null, null, instanceId).InstanceId);
		}

		#endregion

		#region Constructors

		[TestCase(@"\\\\?\\usb#vid_2bd9&pid_0011&mi_00#6&a1e91ba&0&0000#{65e8773d-8f56-11d0-a3b9-00a0c9223196}\\global",
			"USB", "VID_2BD9&PID_0011&MI_00", "6&A1E91BA&0&0000")]
		[TestCase(@"USB\VID_2BD9&PID_0011&MI_00\6&A1E91BA&0&0000", "USB", "VID_2BD9&PID_0011&MI_00", "6&A1E91BA&0&0000")]
		[TestCase(@"PCI\VEN_1000&DEV_0001&SUBSYS_00000000&REV_02\1&08", "PCI", "VEN_1000&DEV_0001&SUBSYS_00000000&REV_02", "1&08")]
		public void WindowsDevicePathInfoConstructorTest(string devicePath, string expectedType, string expectedDeviceId, string expectedInstanceId)
		{
			WindowsDevicePathInfo info = new WindowsDevicePathInfo(devicePath);

			Assert.AreEqual(expectedType, info.Type);
			Assert.AreEqual(expectedDeviceId, info.DeviceId);
			Assert.AreEqual(expectedInstanceId, info.InstanceId);
		}

		#endregion

		#region Equality

		[Test]
		public void EqualsTest()
		{
			WindowsDevicePathInfo a = new WindowsDevicePathInfo(@"\\\\?\\usb#vid_2bd9&pid_0011&mi_00#6&a1e91ba&0&0000#{65e8773d-8f56-11d0-a3b9-00a0c9223196}\\global");
			WindowsDevicePathInfo b = new WindowsDevicePathInfo(@"USB\VID_2BD9&PID_0011&MI_00\6&A1E91BA&0&0000");
			WindowsDevicePathInfo c = new WindowsDevicePathInfo(@"PCI\VEN_1000&DEV_0001&SUBSYS_00000000&REV_02\1&08");

			Assert.AreEqual(a, a);
			Assert.AreEqual(b, b);
			Assert.AreEqual(a, b);

			Assert.AreNotEqual(a, c);
			Assert.AreNotEqual(b, c);
		}

		[Test]
		public void CompareToTest()
		{
			WindowsDevicePathInfo a = new WindowsDevicePathInfo(@"\\\\?\\usb#vid_2bd9&pid_0011&mi_00#6&a1e91ba&0&0000#{65e8773d-8f56-11d0-a3b9-00a0c9223196}\\global");
			WindowsDevicePathInfo b = new WindowsDevicePathInfo(@"USB\VID_2BD9&PID_0011&MI_00\6&A1E91BA&0&0000");
			WindowsDevicePathInfo c = new WindowsDevicePathInfo(@"PCI\VEN_1000&DEV_0001&SUBSYS_00000000&REV_02\1&08");

			List<WindowsDevicePathInfo> items = new List<WindowsDevicePathInfo>{a, b, c};
			List<WindowsDevicePathInfo> sorted = items.Order().ToList();
			List<WindowsDevicePathInfo> expected = new List<WindowsDevicePathInfo>{c, b, a};

			Assert.IsTrue(sorted.SequenceEqual(expected));
		}

		#endregion
	}
}
