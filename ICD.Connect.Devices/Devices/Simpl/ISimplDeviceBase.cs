using ICD.Common.Properties;
using ICD.Connect.API.Attributes;
using ICD.Connect.Settings.Originators.Simpl;

namespace ICD.Connect.Devices.Simpl
{
	public interface ISimplDeviceBase : ISimplOriginator, IDeviceBase
	{
		/// <summary>
		/// Gets/sets the device online status.
		/// </summary>
		[PublicAPI("S+")]
		[ApiMethod(SimplDeviceBaseApi.METHOD_SET_IS_ONLINE, SimplDeviceBaseApi.HELP_METHOD_SET_IS_ONLINE)]
		void SetIsOnline(bool online);
	}
}
