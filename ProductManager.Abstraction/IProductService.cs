using ProductManager.Models;

namespace ProductManager.Abstraction
{
    public interface IProductService
    {
        Task<ProductModel> Create(ProductModel model, int userId);

        Task<ProductModel> Read(int Id);

        Task<ProductModels> GetAllByPagination(int pageNumber);

        Task<ProductModels> GetByBarCodeOrPLU(string query);

        Task<ProductModels> GetBasketProducts();

        Task<ProductModel> Update(ProductModel model, int userId);

        Task<ProductModel> AddOrDeleteProductFromBasket(int productId, int userId);

        Task Deltele(int productId);
    }
}