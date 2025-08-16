using AspNetFinalProject.DTOs;
using AspNetFinalProject.Enums;

namespace AspNetFinalProject.Mappers;

public static class EntityTargetTypeMapper
{
    public static EntityTargetTypeDto CreateDto(EntityTargetType entityType)
    {
        return new EntityTargetTypeDto
        {
            Value = entityType,
            Text = entityType switch
            {
                EntityTargetType.Board => "Дошка",
                EntityTargetType.BoardList => "Список",
                EntityTargetType.Card => "Картка",
                EntityTargetType.Workspace => "Робочий простір",
                _ => throw new ArgumentOutOfRangeException(nameof(entityType), entityType, null)
            }
        };
    }
}