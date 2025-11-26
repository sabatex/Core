using System.Threading.Tasks;
using Xunit;
using Sabatex.Core.RadzenBlazor;
using System.Collections.Generic;
using System;

#nullable enable

namespace Sabatex.Core.Tests;

public class ISabatexRadzenBlazorDataAdapterTests
{
    private class DummyEntity : IEntityBase<string>
    {
        public string Id { get; set; } = string.Empty;
        public string ToKeyString() => Id ?? string.Empty;
    }

    private class DummyAdapter : ISabatexRadzenBlazorDataAdapter
    {
        public Task DeleteAsync<TItem, TKey>(TKey id) where TItem : class, IEntityBase<TKey>
        {
            // no-op for test
            return Task.CompletedTask;
        }

        public Task<QueryResult<TItem>> GetAsync<TItem, TKey>(QueryParams queryParams) where TItem : class, IEntityBase<TKey>
        {
            return Task.FromResult(new QueryResult<TItem> { Count = 0, Value = new List<TItem>() });
        }

        public Task<TItem?> GetByIdAsync<TItem, TKey>(TKey id, string? expand = null) where TItem : class, IEntityBase<TKey>
        {
            return Task.FromResult<TItem?>(default);
        }

        public Task<SabatexValidationModel<TItem>> PostAsync<TItem, TKey>(TItem? item) where TItem : class, IEntityBase<TKey>
        {
            return Task.FromResult(new SabatexValidationModel<TItem>(item));
        }

        public Task<SabatexValidationModel<TItem>> UpdateAsync<TItem, TKey>(TItem item) where TItem : class, IEntityBase<TKey>
        {
            return Task.FromResult(new SabatexValidationModel<TItem>(item));
        }

        Task<TItem> ISabatexRadzenBlazorDataAdapter.GetByIdAsync<TItem, TKey>(string id, string expand) where TItem : class
        {
            return Task.FromResult<TItem?>(default); 
        }
    }

    [Fact]
    public async Task NonGeneric_GetAsync_Works()
    {
        ISabatexRadzenBlazorDataAdapter adapter = new DummyAdapter();
        var result = await adapter.GetAsync<DummyEntity, string>(new QueryParams());
        Assert.NotNull(result);
        Assert.Equal(0, result.Count);
    }

    [Fact]
    public async Task NonGeneric_PostAsync_Works()
    {
        ISabatexRadzenBlazorDataAdapter adapter = new DummyAdapter();
        var entity = new DummyEntity { Id = "1" };
        var result = await adapter.PostAsync<DummyEntity, string>(entity);
        Assert.NotNull(result);
        Assert.Equal(entity, result.Result);
    }

    [Fact]
    public async Task NonGeneric_UpdateAsync_Works()
    {
        ISabatexRadzenBlazorDataAdapter adapter = new DummyAdapter();
        var entity = new DummyEntity { Id = "1" };
        var result = await adapter.UpdateAsync<DummyEntity, string>(entity);
        Assert.NotNull(result);
        Assert.Equal(entity, result.Result);
    }

    [Fact]
    public async Task NonGeneric_GetByIdAsync_Works()
    {
        ISabatexRadzenBlazorDataAdapter adapter = new DummyAdapter();
        var result = await adapter.GetByIdAsync<DummyEntity, string>("1");
        Assert.Null(result);
    }

    [Fact]
    public async Task NonGeneric_DeleteAsync_Works()
    {
        ISabatexRadzenBlazorDataAdapter adapter = new DummyAdapter();
        await adapter.DeleteAsync<DummyEntity, string>("1");
    }
}
