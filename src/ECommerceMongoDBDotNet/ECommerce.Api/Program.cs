using ECommerce.Api.Helpers;
using ECommerce.Api.Repositorys.ReadRepository;
using ECommerce.Api.Repositorys.WriteRepository;
using ECommerce.Api.Services.Contract;
using ECommerce.Api.Services;
using ECommerce.Api.MongoWrappers.Contracts;
using ECommerce.Api.MongoWrappers;
using ECommerce.Api.Repositorys.Info;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add services to the container.
builder.Services.Configure<ProductDatabaseSettings>(
    builder.Configuration.GetSection("ProductDatabaseSettings"));

#region [ IOC ]

#region [ Application ]

builder.Services.AddScoped<IProductService, ProductService>();

#endregion [Application]

#region [ Wrapper ]

builder.Services.AddScoped<IMongoWrapper, MongoWrapper>();

#endregion [ Wrapper ]

#region [ Infra - Data ]

builder.Services.AddScoped<IProductReadRepository, ProductReadRepository>();
builder.Services.AddScoped<IProductWriteRepository, ProductWriteRepository>();

#endregion [ Infra - Data EventSourcing ]

#endregion [ IOC ]

var app = builder.Build();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
