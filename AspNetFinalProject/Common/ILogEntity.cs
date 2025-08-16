using AspNetFinalProject.Enums;
using KellermanSoftware.CompareNetObjects;

namespace AspNetFinalProject.Common;

public interface ILogEntity
{
    EntityTargetType GetEntityType();
    string GetName();
    string GetId();
    string GetSettingsLink();
    string GetDescriptionName();
}