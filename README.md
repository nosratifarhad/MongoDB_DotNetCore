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
public interface IMongoWrapper
{
    Task<List<ProductModel>> GetAsync();

    Task<ProductModel?> GetAsync(string id);

    Task CreateAsync(ProductModel productViewModel);

    Task UpdateAsync(string id, ProductModel productViewModel);

    Task RemoveAsync(string id);

}
```

### you should be add dependency injection in Program.cs file :

```csharp
#region [ Wrapper ]

builder.Services.AddScoped<IMongoWrapper, MongoWrapper>();

#endregion [ Wrapper ]
```

### for use collections you should add this code in implementation wrapper

```csharp
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
```


### now you can use collection 

```csharp
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
```

### images : 

### swagger api
![My Remote Image](https://github.com/nosratifarhad/MongoDB/blob/main/imgs/Annotation2.jpg)

### send request for insert in mongodb
![My Remote Image](https://github.com/nosratifarhad/MongoDB/blob/main/imgs/Annotation4.jpg)

### insert in collection mongodb 
![My Remote Image](https://github.com/nosratifarhad/MongoDB/blob/main/imgs/Annotation1.jpg)


