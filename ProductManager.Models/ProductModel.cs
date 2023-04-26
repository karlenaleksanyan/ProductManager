using System.ComponentModel.DataAnnotations;

namespace ProductManager.Models
{
    public class ProductModel : BaseModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string Barcode { get; set; }
        public int PLU { get; set; }

        [Display(Name = "Add Basket")]
        public bool HasBasket { get; set; }

        public string? Image { get; set; }
    }

    public class ProductModels
    {
        public List<ProductModel>? ProductModelList { get; set; }
        public int Count { get; set; }
    }
}