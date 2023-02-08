namespace DateTimeLibrary;

public interface IDateTimeRange
{
	public DateTime Start { get; set; }
	public DateTime End { get; set; }

	public bool CheckDate();
}
