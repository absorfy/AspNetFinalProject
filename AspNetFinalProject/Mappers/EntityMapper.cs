namespace AspNetFinalProject.Mappers;

public abstract class EntityMapper<TEntity, TCreateDto, TUpdateDto, TDto>
{
    public virtual TDto ToDto(TEntity entity)
    {
        ArgumentNullException.ThrowIfNull(entity);
        return MapToDto(entity);
    }
    
    public virtual void UpdateEntity(TEntity entity, TUpdateDto updateDto)
    {
        ArgumentNullException.ThrowIfNull(entity);
        ArgumentNullException.ThrowIfNull(updateDto);
        MapToEntity(entity, updateDto);
    }

    public virtual TEntity CreateEntity(string authorId, TCreateDto createDto)
    {
        ArgumentNullException.ThrowIfNull(createDto);
        return MapToEntity(authorId, createDto);
    }
    
    protected abstract TDto MapToDto(TEntity entity);
    protected abstract void MapToEntity(TEntity entity, TUpdateDto updateDto);
    protected abstract TEntity MapToEntity(string authorId, TCreateDto createDto);
}