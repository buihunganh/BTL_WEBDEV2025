using System.ComponentModel.DataAnnotations;

namespace BTL_WEBDEV2025.Models
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email address")]
        public string Email { get; set; } = string.Empty;

        [Display(Name = "Country/Region")]
        public string Country { get; set; } = "Vietnam";
    }

    public class RegisterViewModel
    {
        [Required]
        [Display(Name = "First name")]
        public string FirstName { get; set; } = string.Empty;

        [Required]
        [Display(Name = "Last name")]
        public string LastName { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        [StringLength(100, MinimumLength = 6)]
        [DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;

        [Display(Name = "Shopping preference")]
        public string Preference { get; set; } = string.Empty;

        [Range(1,31)] public int? BirthDay { get; set; }
        [Range(1,12)] public int? BirthMonth { get; set; }
        [Range(1900,2100)] public int? BirthYear { get; set; }

        [Required]
        [Display(Name = "Accept policies")]
        [Range(typeof(bool), "true", "true", ErrorMessage = "You must agree to continue")]
        public bool AcceptPolicy { get; set; }
    }
}