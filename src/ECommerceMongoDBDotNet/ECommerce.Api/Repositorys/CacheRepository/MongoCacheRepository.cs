using ECommerce.Api.Helpers;
using ECommerce.Api.Repositorys.CacheRepository.Contracts;
using ECommerce.Api.ViewModels.ProductViewModels;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace ECommerce.Api.Repositorys.CacheRepository;

public class MongoCacheRepository : IMongoCacheRepository
{
    #region Fields

    private readonly IMongoCollection<ProductViewModel> _ProductsCollection;
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
    }

    #endregion Ctor

    #region Implement

    public async Task CreateAsync(ProductViewModel productViewModel)
        => await _ProductsCollection.InsertOneAsync(productViewModel);

    public async Task CreateManyAsync(IEnumerable<ProductViewModel> productViewModels)
        => await _ProductsCollection.InsertManyAsync(productViewModels);

    public async Task<List<ProductViewModel>> GetAsync()
        => await _ProductsCollection.Find(_ => true).ToListAsync();

    public async Task<ProductViewModel?> GetAsync(string id)
        => await _ProductsCollection.Find(x => x.Id == id.ToString()).FirstOrDefaultAsync();

    public async Task RemoveAsync(string id)
        => await _ProductsCollection.FindOneAndDeleteAsync(p => p.Id == id);

    #endregion Implement

}
