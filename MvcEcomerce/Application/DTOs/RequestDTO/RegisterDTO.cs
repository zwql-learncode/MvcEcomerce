using System.ComponentModel.DataAnnotations;

namespace MvcEcomerce.Application.DTOs.RequestDTO
{
    public class RegisterDTO
    {
        [Required(ErrorMessage = "*")]
        [MaxLength(20, ErrorMessage = "Id can not longer than 20 characters")]
        public string Id { get; set; } = string.Empty;
        [Required(ErrorMessage = "*")]
        public string Password { get; set; } = string.Empty;
        [MaxLength(50, ErrorMessage = "Full name can not longer than 50 characters")]
        public string FullName { get; set; } = string.Empty;
        public bool Sex { get; set; } = true;
        [DataType(DataType.Date)]
        public DateTime? DateOfBirth { get; set; }
        [MaxLength(60, ErrorMessage = "Address can not longer than 60 characters")]
        public string Address { get; set; } = string.Empty;
        [Required(ErrorMessage = "Phone number is required"), RegularExpression(@"^(\+?\d{1,3})?0?\d{9}$", ErrorMessage = "Phone number is not valid")]
        public string Phone { get; set; } = string.Empty;
        [EmailAddress(ErrorMessage = "Email is not valid")]
        public string Email { get; set; } = string.Empty;
    }
}
