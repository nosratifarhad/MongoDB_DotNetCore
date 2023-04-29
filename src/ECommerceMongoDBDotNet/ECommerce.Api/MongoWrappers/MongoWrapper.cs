using ECommerce.Api.Helpers;
using ECommerce.Api.MongoWrappers.Contracts;
using ECommerce.Service.ViewModels.ProductViewModels;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace ECommerce.Api.MongoWrappers;

public class MongoWrapper : IMongoWrapper
{
    #region Fields

    private readonly IMongoCollection<ProductModel> _ProductsCollection;
    #endregion Fields

    #region Ctor

    public MongoWrapper(IOptions<ProductDatabaseSettings> settings)
    {
        var mongoClient = new MongoClient(
      settings.Value.ConnectionString);

        var mongoDatabase = mongoClient.GetDatabase(
            settings.Value.DatabaseName);

        _ProductsCollection = mongoDatabase.GetCollection<ProductModel>(
            settings.Value.ProductsCollectionName);
    }

    #endregion Ctor

    #region Implement

    public async Task CreateAsync(ProductModel productViewModel)
        => await _ProductsCollection.InsertOneAsync(productViewModel);

    public async Task<List<ProductModel>> GetAsync()
        => await _ProductsCollection.Find(_ => true).ToListAsync();

    public async Task<ProductModel?> GetAsync(string id)
        => await _ProductsCollection.Find(x => x.ProductId == id).FirstOrDefaultAsync();

    public async Task RemoveAsync(string id)
        => await _ProductsCollection.DeleteOneAsync(id);

    public Task UpdateAsync(string id, ProductModel productViewModel)
        => _ProductsCollection.ReplaceOneAsync(p => p.ProductId == id, productViewModel);

    #endregion Implement

}
