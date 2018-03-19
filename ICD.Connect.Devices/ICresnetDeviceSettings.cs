namespace ICD.Connect.Devices
{
	public interface ICresnetDeviceSettings
	{
		byte? CresnetId { get; set; }
		int? BranchId { get; set; }
		int? ParentId { get; set; }
	}
}