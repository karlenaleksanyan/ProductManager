namespace ProductManager.DBMap.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string Barcode { get; set; }
        public int PLU { get; set; }
        public string? Image { get; set; }
        public bool? HasBasket { get; set; } = false;
        public bool IsActive { get; set; }
        public long? CreateUser { get; set; }
        public DateTime CreateDate { get; set; }
        public long? ModifyUser { get; set; }
        public DateTime? ModifyDate { get; set; }
    }
}