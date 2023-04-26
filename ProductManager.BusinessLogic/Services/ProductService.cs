using ProductManager.Abstraction;
using ProductManager.DBMap.Models;
using ProductManager.Models;
using ProductManager.Repository.Abstraction;

namespace ProductManager.BusinessLogic.Services
{
    internal class ProductService : IProductService
    {
        private readonly IProductRepository productRepository;

        public ProductService(IProductRepository productRepository)
        {
            this.productRepository = productRepository;
        }

        public async Task<ProductModel> Create(ProductModel model, int userId)
        {

            try
            {
                var product = await productRepository.GetByName(model.Name);

                if (product == null)
                {
                    await productRepository.Create(model, userId);
                }
                else
                {
                    model.Id = product.Id;
                    await productRepository.Update(model, userId);
                    model.IsValid = false;
                    model.ErrorType = Enums.ErrorTypes.Warning;
                    model.FriendlyErrorMsg = "Sorry but you cannot re-add from the same product\n.Each time generating items, the old ones, if exist,\nshould be deleted.";
                }
            }
            catch (Exception ex)
            {
                model.IsValid = false;
                model.ErrorType = Enums.ErrorTypes.ServerError;
                model.DeveloperErrorMsg = ex.Message;
            }


            return model;
        }

        public async Task Deltele(int Id)
        {
            await productRepository.Deltele(Id);
        }

        public async Task<ProductModels> GetAllByPagination(int pageNumber)
        {
            return await productRepository.GetAllByPagination(pageNumber);
        }

        public async Task<ProductModels> GetByBarCodeOrPLU(string query)
        {
            decimal num = 0;

            if (!string.IsNullOrEmpty(query) && !decimal.TryParse(query, out num))
            {
                return null;
            };

            return await productRepository.GetByBarCodeOrPLU(num);
        }

        public async Task<ProductModels> GetBasketProducts()
        {
            return await productRepository.GetBasketProducts();
        }

        public async Task<ProductModel> Read(int Id)
        {
            var product = await productRepository.Read(Id);

            if (product == null)
            {
                product.IsValid = false;
                product.ErrorType = Enums.ErrorTypes.None;
                product.FriendlyErrorMsg = "This product is not valid";
            }
            return product;
        }

        public async Task<ProductModel> Update(ProductModel model, int userId)
        {
            var product = await productRepository.Read(model.Id);

            if (product != null)
            {
                model.HasBasket=product.HasBasket;
                product = await productRepository.Update(model, userId);
            }
            else
            {
                product.IsValid = false;
                product.ErrorType = Enums.ErrorTypes.None;
                product.FriendlyErrorMsg = "This product has not exist";
            }

            return product;
        }

        public async Task<ProductModel> AddOrDeleteProductFromBasket(int productId, int userId)
        {
            var product = await productRepository.Read(productId);

            if (product != null)
            {
                product.HasBasket = !product.HasBasket;
                product = await productRepository.Update(product, userId);
            }
            else
            {
                product.IsValid = false;
                product.ErrorType = Enums.ErrorTypes.None;
                product.FriendlyErrorMsg = "This product is not valid";
            }

            return product;
        }
    }
}