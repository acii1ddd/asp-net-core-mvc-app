namespace lab_3.Models
{
    public class TicketViewModel
    {
        public int Id { get; set; }

        public string Source { get; set; } = string.Empty;

        public string Destination { get; set; } = string.Empty;

        public DateTime StartTime { get; set; }

        public DateTime ArrivalTime { get; set; }

        public int WagonNumber { get; set; }

        public int PlaceNumber { get; set; }
    }
}
