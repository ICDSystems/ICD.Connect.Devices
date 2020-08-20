using ICD.Connect.API.EventArguments;
using ICD.Connect.API.Info;
using ICD.Connect.Devices.Simpl;

namespace ICD.Connect.Devices.EventArguments
{
	public sealed class RequestShimResyncEventArgs : AbstractApiEventArgs
	{
		/// <summary>
		/// Constructor.
		/// </summary>
		public RequestShimResyncEventArgs() : base(SimplDeviceBaseApi.EVENT_ON_REQUEST_SHIM_RESYNC)
		{
		}

		/// <summary>
		/// Builds an API result for the args.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="result"></param>
		/// <returns></returns>
		public override void BuildResult(object sender, ApiResult result)
		{
		}
	}
}