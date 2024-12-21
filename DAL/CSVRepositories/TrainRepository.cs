using DAL.Entities;
using DAL.Interfaces;

namespace DAL.CSVRepositories
{
    public class TrainRepository : ITrainRepository
    {
        private string FileName;

        public TrainRepository(string FileName)
        {
            this.FileName = FileName;
        }

        public void Add(Train entity)
        {
            File.AppendAllText(FileName, $"{entity.Id};{entity.Route};{entity.Capacity};{entity.WagonCount}\n");
        }

        public void Delete(Train entity)
        {
            var lines = File.ReadAllLines(FileName).ToList();
            string lineToDelete = lines.FirstOrDefault(line => line.StartsWith(entity.Id.ToString() + ';'));
            if (lineToDelete != null)
            {
                lines.Remove(lineToDelete);
                File.WriteAllLines(FileName, lines);
            }
        }

        public Train Get(int id)
        {
            var lines = File.ReadAllLines(FileName).ToList();
            string lineToGet = lines.FirstOrDefault(line => line.StartsWith(id.ToString() + ';'));
            if (lineToGet != null)
            {
                string[] trainToGet = lineToGet.Split(';');
                return new Train
                {
                    Id = int.Parse(trainToGet[0]),
                    Route = trainToGet[1],
                    Capacity = int.Parse(trainToGet[2]),
                    WagonCount = int.Parse(trainToGet[3])
                };
            }
            else
            {
                throw new Exception("Поезд не найден.");
            }
        }

        public IEnumerable<Train> GetAll()
        {
            var trains = new List<Train>();
            using (var sr = new StreamReader(FileName))
            {
                while (!sr.EndOfStream)
                {
                    var line = sr.ReadLine();
                    string[] trainString = line.Split(';');
                    trains.Add(new Train
                    {
                        Id = int.Parse(trainString[0]),
                        Route = trainString[1],
                        Capacity = int.Parse(trainString[2]),
                        WagonCount = int.Parse(trainString[3]),
                    });
                }
            }
            return trains;
        }

        public void Update(Train entity)
        {
            List<Train> currentTrains = this.GetAll().ToList();
            Train trainToUpdate = currentTrains.FirstOrDefault(tick => tick.Id == entity.Id);
            if (trainToUpdate != null)
            {
                trainToUpdate.Route = entity.Route;
                trainToUpdate.Capacity = entity.Capacity;
                trainToUpdate.WagonCount = entity.WagonCount;

                File.WriteAllText(FileName, string.Empty);
                foreach (var train in currentTrains)
                {
                    this.Add(train);
                }
            }
            else
            {
                throw new Exception($"Пассажир с id ={entity.Id} не найден.");
            }
        }
    }
}
