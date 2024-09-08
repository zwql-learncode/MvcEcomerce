namespace MvcEcomerce.Application.DTOs.ResponseDTO
{
    public class ProductDetailResponseDTO
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string ImageUrl {get; set;} = string.Empty;
        public double Price { get; set;}
        public string ShortDescription { get; set;} = string.Empty;
        public string CategoryName { get; set;} = string.Empty; 
        public string Description { get; set;} = string.Empty;
        public int Point { get; set; } = 0;
        public int Quantity { get; set; } = 0;
    }
}
