using Microsoft.AspNetCore.Mvc;
using ProductManager.Abstraction;
using ProductManager.Models;
using System.Web;

namespace ProductManager.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductService productService;

        public ProductController(IProductService productService)
        {
            this.productService = productService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            ProductModels products = await productService.GetAllByPagination(1);
            ViewBag.actionName = "Index";
            return View(products);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(ProductModel product)
        {
            var productModel = await productService.Create(product, 1);

            if (!product.IsValid)
            {
                if (product.ErrorType == Enums.ErrorTypes.Warning)
                {
                    TempData["Message"] = productModel.FriendlyErrorMsg;
                }
                else
                {
                    TempData["ErrorMessage"] = productModel.DeveloperErrorMsg;
                }
            }

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Edit(int id)
        {
            var product = await productService.Read(id);

            if (product == null)
            {
                TempData["ErrorMessage"] = "This Product has not exist";
                return View("Index");
            }

            ViewBag.previousAction = Request.Headers.Referer.ToString().Split("/").
                                                LastOrDefault()?.Replace("?", "");

            return View(product);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(ProductModel product, string previousAction)
        {
            var productModel = await productService.Update(product, 1);

            if (productModel.IsValid)
            {
                if (string.IsNullOrEmpty(previousAction))
                {
                    previousAction = "Index";
                }
                return RedirectToAction(HttpUtility.UrlDecode(previousAction));
            }
            else
            {
                if (product.ErrorType == Enums.ErrorTypes.Warning)
                {
                    TempData["Message"] = productModel.FriendlyErrorMsg;
                }
                else
                {
                    TempData["ErrorMessage"] = productModel.DeveloperErrorMsg;
                }
            }

            return View("Index");
        }

        public async Task<IActionResult> Delete(int id)
        {
            var productModel = await productService.Read(id);

            if (!productModel.IsValid)
            {
                if (productModel.ErrorType == Enums.ErrorTypes.Warning)
                {
                    TempData["Message"] = productModel.FriendlyErrorMsg;
                }
                else
                {
                    TempData["ErrorMessage"] = productModel.DeveloperErrorMsg;
                }
                return RedirectToAction("Index");
            }

            return View(productModel);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(ProductModel productModel)
        {
            ProductModel product = await productService.Read(productModel.Id);

            if (!product.IsValid)
            {
                if (product.ErrorType == Enums.ErrorTypes.Warning)
                {
                    TempData["Message"] = productModel.FriendlyErrorMsg;
                }
                else
                {
                    TempData["ErrorMessage"] = productModel.DeveloperErrorMsg;
                }
            }

            await productService.Deltele(productModel.Id);

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Details(int id)
        {
            ProductModel product = await productService.Read(id);

            if (!product.IsValid)
            {
                if (product.ErrorType == Enums.ErrorTypes.Warning)
                {
                    TempData["Message"] = product.FriendlyErrorMsg;
                }
                else
                {
                    TempData["ErrorMessage"] = product.DeveloperErrorMsg;
                }
            }

            return View(product);
        }

        [HttpGet]
        public async Task<IActionResult> Search(string query)
        {
            var products = await productService.GetByBarCodeOrPLU(query);

            if (products == null)
            {
                TempData["ErrorMessage"] = "You can search by only number format";
                return View("Index");
            }

            return View("Index", products);
        }

        [HttpGet]
        public async Task<IActionResult> GetBasketProducts()
        {
            var products = await productService.GetBasketProducts();

            if (products == null || products.Count == 0)
            {
                TempData["Message"] = "you don't have product in a basket";
                return View("Index");
            }

            return View("Index", products);
        }

        [HttpGet]
        public async Task<ActionResult> AddOrDeleteProductFromBasket(int id)
        {
            var productModel = await productService.AddOrDeleteProductFromBasket(id, 1);

            if (!productModel.IsValid)
            {
                TempData["ErrorMessage"] = productModel.FriendlyErrorMsg;
                return View("Index");
            }

            if (!productModel.HasBasket)
            {
                return RedirectToAction("GetBasketProducts");
            }

            return Ok();
        }

        public async Task<IActionResult> GetPage(int pageNumber)
        {
            var products = await productService.GetAllByPagination(pageNumber);

            return PartialView("_Product", products.ProductModelList);
        }
    }
}