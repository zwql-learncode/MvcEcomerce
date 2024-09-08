using System.ComponentModel.DataAnnotations;

namespace MvcEcomerce.Application.DTOs.RequestDTO
{
    public class LoginDTO
    {

        [Required(ErrorMessage = "Customer id is required")]
        public string Id { get; set; } = string.Empty;
        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; } = string.Empty;
    }
}
