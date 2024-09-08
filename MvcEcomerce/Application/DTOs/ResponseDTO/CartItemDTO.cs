namespace MvcEcomerce.Application.DTOs.ResponseDTO
{
    public class CartItemDTO
    {
        public int Id { get; set; }
        public string ImageUrl { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public double Price { get; set; } 
        public int Quantity { get; set; }
        public double Amount => Price * Quantity;
    }
}
