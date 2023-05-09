﻿using ECommerce.Api.ViewModels.ProductViewModels;

namespace ECommerce.Api.Repositorys.CacheRepository.Contracts;

public interface IMongoCacheRepository
{
    Task<List<ProductViewModel>> GetAsync();

    Task<ProductViewModel> GetAsync(string id);

    Task CreateAsync(ProductViewModel product);

    Task CreateManyAsync(IEnumerable<ProductViewModel> productViewModels);

    Task RemoveAsync(string id);
}
