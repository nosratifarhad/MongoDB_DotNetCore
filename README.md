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

### you need Mongo Repository for use mongodb collection from service

```csharp
public interface IMongoCacheRepository
{
    Task<List<ProductViewModel>> GetAsync();

    Task<ProductViewModel> GetAsync(string id);

    Task<List<ProductViewModel>> GetProductByFilterAsync(string id, string productName, string productTitle);

    Task CreateAsync(ProductViewModel product);

    Task CreateAsync(List<ProductViewModel> results);

    Task CreateManyAsync(IEnumerable<ProductViewModel> productViewModels);

    Task RemoveAsync(string id);
}
```

### you should be add dependency injection in Program.cs file :

```csharp
#region [ Cache  ]

builder.Services.AddScoped<IMongoCacheRepository, MongoCacheRepository>();

#endregion [ Cache  ]
```

### for use collections you should add this code in implementation Cache Repository

```csharp
    #region Fields

    private readonly IMongoCollection<ProductModel> _ProductsCollection;
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
```


### now you can use collection 

```csharp
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
```

### images : 

### swagger api
![My Remote Image](https://github.com/nosratifarhad/MongoDB/blob/main/imgs/Annotation2.jpg)

### send request for insert in mongodb
![My Remote Image](https://github.com/nosratifarhad/MongoDB/blob/main/imgs/Annotation4.jpg)

### insert in collection mongodb 
![My Remote Image](https://github.com/nosratifarhad/MongoDB/blob/main/imgs/Annotation1.jpg)


