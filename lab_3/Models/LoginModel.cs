using System.ComponentModel.DataAnnotations;

namespace lab_3.Models
{
    public class LoginModel
    {
        [Required(ErrorMessage = "Не задано имя пользоватетя")]
        public string UserName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Не задан пароль пользователя")]
        [DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;
    }
}
