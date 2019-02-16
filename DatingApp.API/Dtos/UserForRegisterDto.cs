using System.ComponentModel.DataAnnotations;

namespace DatingApp.API.Dtos
{
    public class UserForRegisterDto
    {
        [Required]
        public string Username { get; set; }

        [Required]
        // password length check
        [StringLength(8, MinimumLength = 4, ErrorMessage="Please provice password minimum 4 character ")]
        public string Password { get; set; }
    }
}