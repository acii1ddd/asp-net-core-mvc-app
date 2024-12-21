namespace DAL.Entities
{
    public class Train
    {
        public int Id { get; set; }
        
        public string Route { get; set; } = string.Empty;
        
        public int Capacity { get; set; }
        
        public int WagonCount { get; set; }
    }
}
