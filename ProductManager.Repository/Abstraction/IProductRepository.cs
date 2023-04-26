using ProductManager.Models;

namespace ProductManager.Repository.Abstraction
{
    public interface IProductRepository
    {
        Task Create(ProductModel model, int userId);

        Task<ProductModel> Read(int Id);

        Task<ProductModels> GetAllByPagination(int pageNumber);

        Task<ProductModels> GetByBarCodeOrPLU(decimal num);

        Task<ProductModels> GetBasketProducts();

        Task<ProductModel> GetByName(string name);

        Task<ProductModel> Update(ProductModel model, int userId);

        Task Deltele(int Id);
    }
}