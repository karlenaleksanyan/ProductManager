using Microsoft.EntityFrameworkCore;
using ProductManager.DBMap;
using ProductManager.DBMap.Models;
using ProductManager.Models;
using ProductManager.Repository.Abstraction;

namespace ProductManager.Repository
{
    internal class ProductRepository : IProductRepository
    {
        private readonly ProductManagerDbContext context;

        public ProductRepository(ProductManagerDbContext context)
        {
            this.context = context;
        }

        public async Task<ProductModels> GetAllByPagination(int pageNumber)
        {
            int pageSize = 48;
            var result = context.Products.Skip((pageNumber - 1) * pageSize).Take(pageSize);

            var products = await result.Select(x => new ProductModel
            {
                Id = x.Id,
                Name = x.Name,
                Barcode = x.Barcode,
                PLU = x.PLU,
                Image = x.Image,
                Price = x.Price,
                HasBasket = x.HasBasket ?? false
            }).ToListAsync();

            var productList = new ProductModels
            {
                ProductModelList = products,
                Count = context.Products.Count()
            };
            return productList;
        }

        public async Task Create(ProductModel model, int userId)
        {
            var product = new Product
            {
                Name = model.Name,
                Barcode = model.Barcode,
                PLU = model.PLU,
                Image = model.Image,
                Price = model.Price,
                HasBasket = model.HasBasket,
                CreateDate = DateTime.Now,
                CreateUser = userId
            };
            await context.Products.AddAsync(product);
            await context.SaveChangesAsync();
        }

        public async Task Deltele(int Id)
        {
            var product = context.Products.SingleOrDefault(x => x.Id == Id);
            context.Products.Remove(product);
            await context.SaveChangesAsync();
        }



        public async Task<ProductModels> GetByBarCodeOrPLU(decimal num)
        {
            var products = await context.Products.Where(x => num == 0 || Convert.ToInt64(x.Barcode) == num || x.PLU == num).Select(x => new ProductModel
            {
                Id = x.Id,
                Name = x.Name,
                Barcode = x.Barcode,
                PLU = x.PLU,
                Image = x.Image,
                Price = x.Price,
                HasBasket = x.HasBasket ?? false
            }).AsNoTracking().ToListAsync();

            return new ProductModels
            {
                ProductModelList = products,
                Count = products.Count()
            };
        }
        public async Task<ProductModels> GetBasketProducts()
        {
            var products = await context.Products.Where(x => x.HasBasket.Value).Select(x => new ProductModel
            {
                Id = x.Id,
                Name = x.Name,
                Barcode = x.Barcode,
                PLU = x.PLU,
                Image = x.Image,
                Price = x.Price,
                HasBasket = x.HasBasket ?? false
            }).AsNoTracking().ToListAsync();

            return new ProductModels
            {
                ProductModelList = products,
                Count = products.Count()
            };
        }
        public async Task<ProductModel> GetByName(string name)
        {
            var product = await context.Products.AsNoTracking().SingleOrDefaultAsync(p => p.Name == name);

            ProductModel productModel = default;
            if (product != null)
            {
                productModel = new ProductModel
                {
                    Id = product.Id,
                    Name = product.Name,
                    Barcode = product.Barcode,
                    PLU = product.PLU,
                    Image = product.Image,
                    Price = product.Price,
                    HasBasket = product.HasBasket ?? false
                };
            }
            return productModel;
        }


        public async Task<ProductModel> Read(int Id)
        {
            var product = await context.Products.AsNoTracking().SingleOrDefaultAsync(p => p.Id == Id);

            ProductModel productModel = default;

            if (product != null)
            {
                productModel = new ProductModel
                {
                    Id = product.Id,
                    Name = product.Name,
                    Barcode = product.Barcode,
                    PLU = product.PLU,
                    Image = product.Image,
                    Price = product.Price,
                    HasBasket = product.HasBasket ?? false
                };
            }
            return productModel;
        }

        public async Task<ProductModel> Update(ProductModel model, int userId)
        {
            var product = new Product
            {
                Id = model.Id,
                Name = model.Name,
                Barcode = model.Barcode,
                PLU = model.PLU,
                Image = model.Image,
                Price = model.Price,
                HasBasket = model.HasBasket,
                ModifyUser = userId,
                ModifyDate = DateTime.Now
            };

            context.Products.Update(product);
            await context.SaveChangesAsync();

            return model;
        }
    }
}