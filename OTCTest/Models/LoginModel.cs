using System.ComponentModel.DataAnnotations;

namespace OTCTest.Models
{
    public class LoginModel
    {
        [Required(ErrorMessage = "Укажите логин !")]
        public string Login { get; set; }

        [Required(ErrorMessage = "Укажите пароль !")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        public string Status { get; set; }
    }
}
