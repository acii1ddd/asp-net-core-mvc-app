namespace DAL.Entities
{
    public class Passenger
    {
        public int Id { get; set; } 

        public string FirstName { get; set; } = string.Empty;
        
        public string SecondName { get; set; } = string.Empty;
        
        public string Phone { get; set; } = string.Empty;
        
        public string PassportData { get; set; } = string.Empty;
    }
}
