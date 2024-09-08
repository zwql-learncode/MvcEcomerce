namespace MvcEcomerce.Application.DTOs.ResponseDTO
{
    public class CategoryResponseDTO
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public int Quantity { get; set; } = 0;
    }
}
