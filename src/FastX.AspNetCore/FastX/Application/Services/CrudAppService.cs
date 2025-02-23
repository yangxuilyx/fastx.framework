using FastX.Data.Entities;
using FastX.Data.Repository;

namespace FastX.Application.Services;

public abstract class CrudAppService<TEntity, TKey, TEntityDto, TGetListInput> : CrudAppService<TEntity, TKey, TEntityDto, TGetListInput,
    TEntityDto, TEntityDto>
    where TEntity : class, IEntity
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="repository"></param>
    protected CrudAppService(IRepository<TEntity> repository) : base(repository)
    {
    }
}

public abstract class CrudAppService<TEntity, TKey, TEntityDto, TGetListInput, TCreateInput> : CrudAppService<TEntity, TKey, TEntityDto, TGetListInput,
    TCreateInput, TCreateInput>
    where TEntity : class, IEntity
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="repository"></param>
    protected CrudAppService(IRepository<TEntity> repository) : base(repository)
    {
    }
}

public abstract class CrudAppService<TEntity, TKey, TEntityDto, TGetListInput, TCreateInput, TUpdateInput> : ReadOnlyAppService<TEntity, TKey, TEntityDto, TGetListInput>
    where TEntity : class, IEntity
{
    /// <summary>
    /// CrudAppService
    /// </summary>
    /// <param name="repository"></param>
    protected CrudAppService(IRepository<TEntity> repository) : base(repository) { }

    public virtual async Task<TEntityDto> InsertAsync(TCreateInput input)
    {
        var entity = await MapCreateDtoToEntity(input);
        entity = await Repository.InsertAsync(entity);

        return await MapToEntityDto(entity);
    }

    public virtual async Task<TEntityDto> UpdateAsync(TUpdateInput input)
    {
        var entity = await MapUpdateDtoToEntity(input);
        entity = await Repository.UpdateAsync(entity);

        return await MapToEntityDto(entity);
    }

    public virtual async Task<TEntityDto> InsertOrUpdateAsync(TCreateInput input)
    {
        var entity = await MapCreateDtoToEntity(input);
        entity = await Repository.InsertOrUpdateAsync(entity);

        return await MapToEntityDto(entity);
    }

    public virtual async Task<bool> DeleteAsync(TKey id)
    {
        return await Repository.DeleteAsync(id);
    }

    protected virtual Task<TEntity> MapToEntity(TEntityDto entityDto)
    {
        return Task.FromResult(ObjectMapper.Map<TEntity>(entityDto));
    }

    protected virtual Task<TEntity> MapUpdateDtoToEntity(TUpdateInput entityDto)
    {
        return Task.FromResult(ObjectMapper.Map<TEntity>(entityDto));
    }

    protected virtual Task<TEntity> MapCreateDtoToEntity(TCreateInput entityDto)
    {
        return Task.FromResult(ObjectMapper.Map<TEntity>(entityDto));
    }
}