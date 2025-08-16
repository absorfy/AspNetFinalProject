namespace AspNetFinalProject.Common;

public class EntityUpdateLog
{
    public string ValueName { get; init; }
    public string OldValue { get; set; }
    public string NewValue { get; set; }
}