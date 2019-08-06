using ICD.Connect.Settings.Originators;

namespace ICD.Connect.Devices.Points
{
	public interface IPoint : IOriginator
	{
		/// <summary>
		/// Device id
		/// </summary>
		int DeviceId { get; set; }

		/// <summary>
		/// Control id.
		/// </summary>
		int ControlId { get; set; }
	}
}