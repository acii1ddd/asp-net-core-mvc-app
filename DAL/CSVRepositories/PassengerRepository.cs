using DAL.EF;
using DAL.Entities;
using DAL.Interfaces;

namespace DAL.CSVRepositories
{
    public class PassengerRepository : IPassengerRepository
    {
        private string FileName; // свой-во для FileName

        public PassengerRepository(string FileName)
        {
            this.FileName = FileName;
        }

        public void Add(Passenger entity)
        {
            File.AppendAllText(FileName, $"{entity.Id};{entity.FirstName};{entity.SecondName};{entity.PassportData};{entity.Phone}\n");
        }

        public void Delete(Passenger entity)
        {
            var lines = File.ReadAllLines(FileName).ToList();
            string lineToDelete = lines.FirstOrDefault(line => line.StartsWith(entity.Id.ToString() + ';'));
            if (lineToDelete != null)
            {
                lines.Remove(lineToDelete);
                File.WriteAllLines(FileName, lines);
            }
        }

        public Passenger Get(int id)
        {
            var lines = File.ReadAllLines(FileName).ToList();
            string lineToGet = lines.FirstOrDefault(line => line.StartsWith(id.ToString() + ';'));
            if (lineToGet != null)
            {
                string[] passengerToGet = lineToGet.Split(';');
                return new Passenger
                {
                    Id = int.Parse(passengerToGet[0]),
                    FirstName = passengerToGet[1],
                    SecondName = passengerToGet[2],
                    Phone = passengerToGet[3],
                    PassportData = passengerToGet[4]
                };
            }
            else
            {
                throw new Exception("Пассажир не найден.");
            }
        }

        public IEnumerable<Passenger> GetAll()
        {
            var passengers = new List<Passenger>();
            using (var sr = new StreamReader(FileName))
            {
                while (!sr.EndOfStream)
                {
                    var line = sr.ReadLine();
                    string[] passengerString = line.Split(';');
                    passengers.Add(new Passenger
                    {
                        Id = int.Parse(passengerString[0]),
                        FirstName = passengerString[1],
                        SecondName = passengerString[2],
                        Phone = passengerString[3],
                        PassportData = passengerString[4]
                    });
                }
            }
            return passengers;
        }

        public void Update(Passenger entity)
        {
            List<Passenger> currentPassengers = this.GetAll().ToList();
            Passenger passengerToUpdate = currentPassengers.FirstOrDefault(pass => pass.Id == entity.Id);
            if (passengerToUpdate != null)
            {
                passengerToUpdate.FirstName = entity.FirstName;
                passengerToUpdate.SecondName = entity.SecondName;
                passengerToUpdate.Phone = entity.Phone;
                passengerToUpdate.PassportData = entity.PassportData;

                File.WriteAllText(FileName, string.Empty);
                foreach (var passenger in currentPassengers)
                {
                    this.Add(passenger);
                }
            }
            else
            {
                throw new Exception($"Пассажир с id ={ entity.Id } не найден.");
            }
        }
    }
}
