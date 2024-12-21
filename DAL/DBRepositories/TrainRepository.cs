using DAL.Entities;
using DAL.Interfaces;
using System.Data.SqlClient;

namespace DAL.DBRepositories
{
    public class TrainRepository : ITrainRepository
    {
        private string connectionString;
        private SqlConnection connection;

        public TrainRepository(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public void Add(Train entity)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = connection.CreateCommand();
                command.CommandText = "insert into trains (Route, Capacity, WagonCount) values " +
                                      "(@Route, @Capacity, @WagonCount)";
                command.Parameters.AddWithValue("@Route", entity.Route);
                command.Parameters.AddWithValue("@Capacity", entity.Capacity);
                command.Parameters.AddWithValue("@WagonCount", entity.WagonCount);
                command.ExecuteNonQuery();
            }
        }

        public void Delete(Train entity)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = connection.CreateCommand();
                command.CommandText = "DELETE FROM trains WHERE Id = @Id";
                command.Parameters.AddWithValue("@Id", entity.Id);
                command.ExecuteNonQuery();

                // сброс идент
                int lastTrainId = this.GetAll().ToList().LastOrDefault().Id;

                SqlCommand resetIdentCommand = new SqlCommand("DBCC CHECKIDENT ('trains', RESEED, @newValue)", connection);
                resetIdentCommand.Parameters.AddWithValue("@newValue", lastTrainId);
                resetIdentCommand.ExecuteNonQuery();
            }
        }

        public Train Get(int id)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = connection.CreateCommand();
                command.CommandText = "SELECT * FROM trains WHERE Id = @Id";
                command.Parameters.AddWithValue("@Id", id);
                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    return new Train
                    {
                        Id = reader.GetInt32(0),
                        Route = reader.GetString(1),
                        Capacity = reader.GetInt32(2),
                        WagonCount = reader.GetInt32(3)
                    };
                }
                else
                {
                    throw new Exception("Поезд не найден.");
                }
            }
        }

        public IEnumerable<Train> GetAll()
        {
            var trains = new List<Train>();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand cmd = connection.CreateCommand();
                cmd.CommandText = "select * from trains";
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    trains.Add(new Train
                    {
                        Id = reader.GetInt32(0),
                        Route = reader.GetString(1),
                        Capacity = reader.GetInt32(2),
                        WagonCount = reader.GetInt32(3)
                    });
                }
            }
            return trains;
        }

        public void Update(Train entity)
        {
            throw new NotImplementedException();
        }
    }
}
