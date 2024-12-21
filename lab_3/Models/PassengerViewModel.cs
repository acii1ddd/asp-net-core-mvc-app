using System.ComponentModel.DataAnnotations;

namespace lab_3.Models
{
    public class PassengerViewModel
    {   
        public int Id { get; set; }

        // аттрибут
        [Required]
        public string FirstName { get; set; } = string.Empty;

        [Required]
        public string SecondName { get; set; } = string.Empty;

        [Required]
        public string Phone { get; set; } = string.Empty;

        [Required]
        public string PassportData { get; set; } = string.Empty;
    }
}
