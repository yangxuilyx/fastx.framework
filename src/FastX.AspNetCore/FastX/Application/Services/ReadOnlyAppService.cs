using AutoMapper;
using FastX.Application.Dtos;
using FastX.Data.Entities;
using FastX.Data.PagedResult;
using FastX.Data.Repository;
using FastX.Data.SqlSugar;
using Microsoft.Extensions.DependencyInjection;
using SqlSugar;

namespace FastX.Application.Services;

public abstract class ReadOnlyAppService<TEntity, TKey, TEntityDto, TGetListInput> : ApplicationService
    where TEntity : class, IEntity, new() where TEntityDto : class
{
    protected IRepository<TEntity> Repository { get; }

    /// <summary>
    /// 
    /// </summary>
    protected ReadOnlyAppService(IRepository<TEntity> repository)
    {
        Repository = repository;
    }

    /// <summary>
    /// Get
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    /// <exception cref="XException"></exception>
    public virtual async Task<TEntityDto> GetAsync(TKey id)
    {
        var entity = await Repository.GetAsync(id);
        if (entity == null)
            throw new XException("实体不存在");

        return await MapToEntityDto(entity);
    }

    /// <summary>
    /// GetList
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public virtual async Task<PagedResultDto<TEntityDto>> ListAsync(TGetListInput input)
    {
        var queryFilter = CreateFilteredQuery(input);
        ApplySorting(queryFilter, input);

        List<TEntity> entities;

        if (input is IPagedResultRequest pagedInput)
        {
            var pageInfo = pagedInput.Page;
            if (pageInfo == null)
                pageInfo = new PageInfo()
                {
                    PageSize = 10,
                    PageIndex = 1,
                };

            RefAsync<int> totalCount = 0;
            RefAsync<int> totalPage = 0;
            entities = await queryFilter
                .ToOffsetPageAsync(pageInfo.PageIndex, pageInfo.PageSize, totalCount, totalPage);

            pageInfo.TotalCount = totalCount;
            pageInfo.TotalPage = totalPage;

            return new PagedResultDto<TEntityDto>(pageInfo, await MapToEntityDtoList(entities));
        }
        else
        {
            entities = await queryFilter.ToListAsync();
            return new PagedResultDto<TEntityDto>()
            {
                Items = await MapToEntityDtoList(entities)
            };
        }
    }

    protected virtual ISugarQueryable<TEntity> ApplySorting(ISugarQueryable<TEntity> query, TGetListInput input)
    {
        if (input is ISortedResultRequest sortedInput)
            return query.OrderByIF(!sortedInput.Sorting.IsNullOrWhiteSpace(), sortedInput.Sorting);

        return query;
    }

    protected virtual ISugarQueryable<TEntity> CreateFilteredQuery(
        TGetListInput input)
    {
        return Repository.GetQueryable();
    }

    protected virtual Task<TEntityDto> MapToEntityDto(TEntity entity)
    {
        return MapTo<TEntity, TEntityDto>(entity);
    }

    protected virtual async Task<List<TEntityDto>> MapToEntityDtoList(List<TEntity> entities, bool includingDetails = false)
    {
        if (!includingDetails)
            return await MapTo<List<TEntityDto>>(entities);

        var entityDtos = await Task.WhenAll(entities.ConvertAll(async entity => await MapToEntityDto(entity)));
        return entityDtos.ToList();
    }

    protected virtual Task<TOutput> MapTo<TOutput>(object? input) where TOutput : class
    {
        return MapTo<object, TOutput>(input);
    }

    protected virtual Task<TOutput> MapTo<TInput, TOutput>(TInput? input) where TInput : class where TOutput : class
    {
        if (input == null)
            return Task.FromResult<TOutput>(default!);

        return Task.FromResult(ObjectMapper.Map<TInput, TOutput>(input));
    }
}