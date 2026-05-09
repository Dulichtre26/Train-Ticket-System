using System.ComponentModel.DataAnnotations;

namespace TrainTicket.Business.DTOs
{
    public class RegisterRequestDto
    {
        [Required, MaxLength(100)]
        public string FullName { get; set; } = string.Empty;

        [Required, EmailAddress, MaxLength(100)]
        public string Email { get; set; } = string.Empty;

        [Required, MinLength(6), MaxLength(100)]
        public string Password { get; set; } = string.Empty;

        [Phone, MaxLength(15)]
        public string? PhoneNumber { get; set; }
    }
}