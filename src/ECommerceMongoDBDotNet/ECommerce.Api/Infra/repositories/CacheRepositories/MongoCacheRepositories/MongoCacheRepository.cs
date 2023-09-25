using ECommerce.Api.Domain;
using ECommerce.Api.Helpers;
using ECommerce.Api.ViewModels.ProductViewModels;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace ECommerce.Api.Infra.Repositories.CacheRepositories.MongoCacheRepositories;

public class MongoCacheRepository : IMongoCacheRepository
{
    #region Fields

    private readonly IMongoCollection<ProductViewModel> _ProductsCollection;
    private readonly FilterDefinitionBuilder<ProductViewModel> _filter;

    #endregion Fields

    #region Ctor

    public MongoCacheRepository(IOptions<ProductDatabaseSettings> settings)
    {
        var mongoClient = new MongoClient(
      settings.Value.ConnectionString);

        var mongoDatabase = mongoClient.GetDatabase(
            settings.Value.DatabaseName);

        _ProductsCollection = mongoDatabase.GetCollection<ProductViewModel>(
            settings.Value.ProductsCollectionName);

        _filter = Builders<ProductViewModel>.Filter;
    }

    #endregion Ctor

    #region Implement

    public async Task CreateAsync(ProductViewModel productViewModel)
        => await _ProductsCollection.InsertOneAsync(productViewModel);

    public async Task CreateAsync(List<ProductViewModel> results)
           => await _ProductsCollection.InsertManyAsync((IEnumerable<ProductViewModel>)results);

    public async Task CreateManyAsync(IEnumerable<ProductViewModel> productViewModels)
        => await _ProductsCollection.InsertManyAsync(productViewModels);

    public async Task<List<ProductViewModel>> GetAsync()
        => await _ProductsCollection.Find(_ => true).ToListAsync();

    public async Task<ProductViewModel?> GetAsync(string id)
        => await _ProductsCollection.Find(x => x.Id == id.ToString()).FirstOrDefaultAsync();

    public async Task<List<ProductViewModel>> GetProductByFilterAsync(string id, string productName, string productTitle)
    {
        var filters = new List<FilterDefinition<ProductViewModel>>();

        FilterDefinition<ProductViewModel> filter;

        if (!string.IsNullOrEmpty(id))
        {
            filters.Add(_filter.Eq(f => f.Id, id));
        }

        if (!string.IsNullOrEmpty(productName))
        {
            filters.Add(_filter.Eq(f => f.ProductName, productName));
        }

        if (!string.IsNullOrEmpty(productTitle))
        {
            filters.Add(_filter.Eq(f => f.ProductTitle, productTitle));
        }

        filter = _filter.And(filters);

        var find = this._ProductsCollection
            .Find(filter);

        var result = await find.ToListAsync();

        return result.ConvertAll<ProductViewModel>(c => c);
    }

    public async Task RemoveAsync(string id)
        => await _ProductsCollection.FindOneAndDeleteAsync(p => p.Id == id);

    #endregion Implement
}