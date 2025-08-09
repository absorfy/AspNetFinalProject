using AspNetFinalProject.Enums;

namespace AspNetFinalProject.Entities;

public interface ILogEntity
{
    EntityTargetType GetEntityType();
    string GetName();
    string GetId();
}