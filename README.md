# MongoDB DotNet 6

## first install package :
```
dotnet add package MongoDB.Driver
```

### add database connection setting in appsettings.json

```json
  "ProductDatabaseSettings": {
    "ConnectionString": "mongodb://localhost:27017",
    "DatabaseName": "Product",
    "ProductsCollectionName": "ProductsCollection"
  },
```

### add setting configure to service

```csharp
// Add services to the container.
builder.Services.Configure<ProductDatabaseSettings>(
    builder.Configuration.GetSection("ProductDatabaseSettings"));
```

### you need wapper for use mongodb collection from service

```csharp
public interface IMongoCacheRepository
{
    Task<List<ProductViewModel>> GetAsync();

    Task<ProductViewModel> GetAsync(string id);

    Task CreateAsync(ProductViewModel product);

    Task CreateManyAsync(IEnumerable<ProductViewModel> productViewModels);

    Task RemoveAsync(string id);
}
```

### you should be add dependency injection in Program.cs file :
```csharp
builder.Services.AddScoped<IMongoCacheRepository, MongoCacheRepository>();
```

### for use collections you should add this code in implementation wrapper

```csharp
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
```

### now you can use collection 

```csharp
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
```

### you can see how to can use mongodb in service layer
------------------------------
### images : 



