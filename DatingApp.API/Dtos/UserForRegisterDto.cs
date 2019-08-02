using System.ComponentModel.DataAnnotations;

namespace DatingApp.API.Dtos
{
    public class UserForRegisterDto
    {
        [Required]
        public string Username { get; set; }

        [Required]
        [StringLength(8,MinimumLength=5,ErrorMessage="You must specify password between 5 and 8")]
        public string Password { get; set; }
    }
}