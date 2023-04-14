using ECommerce.Domain.Products;
using ECommerce.Infra.Repositorys;
using ECommerce.Service.Contract;
using ECommerce.Service.Services;

namespace ECommerce.Api.IOC
{
    public static class NativeInjectorBootStrapper
    {
        public static void RegisterServices(this IServiceCollection services)
        {

            #region [ Application ]

            services.AddScoped<IProductService, ProductService>();

            #endregion [Application]

            #region [ Infra - Data ]

            services.AddScoped<IProductReadRepository, ProductReadRepository>();
            services.AddScoped<IProductWriteRepository, ProductWriteRepository>();

            #endregion [ Infra - Data EventSourcing ]

        }
    }

}
