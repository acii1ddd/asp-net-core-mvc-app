using System.ComponentModel.DataAnnotations;

namespace lab_3.Models
{
    public class UserViewModelToCreate
    {
        //[Required]
        //public string? Id { get; set; }

        //[Required] модель и так не проходит валидацию с null значениями
        // так как string не допускает null (если поставить ? - то ошибки не будет)
        /// <summary>
        /// если нужно ИСПОЛЬЗОВАТЬ КАКОЙ-ТО NULLABLE ТИП - ЛУЧШЕ использовать [Required]
        /// </summary>
        public string UserName { get; set; } = string.Empty;

        public string FirstName { get; set; } = string.Empty;

        public string LastName { get; set; } = string.Empty;

        public string PhoneNumber { get; set; } = string.Empty;

        public string Role { get; set; } = string.Empty;

        [DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;

        //[Required]
        [DataType(DataType.Password)] // при отправки формы прокидывают в ModelState
                                      // ошибку для этого свойтва если валидация не прошла
        [Compare("Password", ErrorMessage = "Пароли не совпадают")]
        public string ConfirmPassword { get; set; } = string.Empty;
    }
}
