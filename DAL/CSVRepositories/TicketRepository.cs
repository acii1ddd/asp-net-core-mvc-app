using DAL.Entities;
using DAL.Interfaces;

namespace DAL.CSVRepositories
{
    public class TicketRepository : ITicketRepository
    {
        private string FileName;

        public TicketRepository(string FileName)
        {
            this.FileName = FileName;
        }

        public void Add(Ticket entity)
        {
            File.AppendAllText(FileName, $"{entity.Id};{entity.Source};{entity.Destination};{entity.StartTime};{entity.ArrivalTime};{entity.WagonNumber};{entity.PlaceNumber}\n");
        }

        public void Delete(Ticket entity)
        {
            var lines = File.ReadAllLines(FileName).ToList();
            string lineToDelete = lines.FirstOrDefault(line => line.StartsWith(entity.Id.ToString() + ';'));
            if (lineToDelete != null)
            {
                lines.Remove(lineToDelete);
                File.WriteAllLines(FileName, lines);
            }
        }

        public Ticket Get(int id)
        {
            var lines = File.ReadAllLines(FileName).ToList();
            string lineToGet = lines.FirstOrDefault(line => line.StartsWith(id.ToString() + ';'));
            if (lineToGet != null)
            {
                string[] ticketToGet = lineToGet.Split(';');
                return new Ticket
                {
                    Id = int.Parse(ticketToGet[0]),
                    Source = ticketToGet[1],
                    Destination = ticketToGet[2],
                    StartTime = DateTime.Parse(ticketToGet[3]),
                    ArrivalTime = DateTime.Parse(ticketToGet[4]),
                    WagonNumber = int.Parse(ticketToGet[5]),
                    PlaceNumber = int.Parse(ticketToGet[6]),
                };
            }
            else
            {
                throw new Exception("Билет не найден.");
            }
        }

        public IEnumerable<Ticket> GetAll()
        {
            var tickets = new List<Ticket>();
            using (var sr = new StreamReader(FileName))
            {
                while (!sr.EndOfStream)
                {
                    var line = sr.ReadLine();
                    string[] ticketString = line.Split(';');
                    tickets.Add(new Ticket
                    {
                        Id = int.Parse(ticketString[0]),
                        Source = ticketString[1],
                        Destination = ticketString[2],
                        StartTime = DateTime.Parse(ticketString[3]),
                        ArrivalTime = DateTime.Parse(ticketString[4]),
                        WagonNumber = int.Parse(ticketString[5]),
                        PlaceNumber = int.Parse(ticketString[6]),
                    });
                }
            }
            return tickets;
        }

        public void Update(Ticket entity)
        {
            List<Ticket> currentTickets = this.GetAll().ToList();
            Ticket ticketToUpdate = currentTickets.FirstOrDefault(tick => tick.Id == entity.Id);
            if (ticketToUpdate != null)
            {
                ticketToUpdate.Source = entity.Source;
                ticketToUpdate.Destination = entity.Destination;
                ticketToUpdate.StartTime = entity.StartTime;
                ticketToUpdate.ArrivalTime = entity.ArrivalTime;
                ticketToUpdate.WagonNumber = entity.WagonNumber;
                ticketToUpdate.PlaceNumber = entity.PlaceNumber;

                File.WriteAllText(FileName, string.Empty);
                foreach (var ticket in currentTickets)
                {
                    this.Add(ticket);
                }
            }
            else
            {
                throw new Exception($"Пассажир с id ={entity.Id} не найден.");
            }
        }
    }
}
