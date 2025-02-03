using System.ComponentModel.DataAnnotations;

namespace Client.Models
{
    public class LoginModel
    {
        public string Email { get; set; }

        public string Password { get; set; }
    }
}
