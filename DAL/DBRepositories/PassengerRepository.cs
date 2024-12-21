using DAL.Entities;
using DAL.Interfaces;
using System.Data.SqlClient;


namespace DAL.DBRepositories
{
    public class PassengerRepository : IPassengerRepository
    {
        public string connectionString;
        //private SqlConnection connection;

        public PassengerRepository(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public void Add(Passenger entity)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = connection.CreateCommand();
                command.CommandText = "insert into passengers (FirstName, SecondName, PassportData, Phone) values " +
                                      "(@FirstName, @SecondName, @Phone, @PassportData)";
                command.Parameters.AddWithValue("@FirstName", entity.FirstName);
                command.Parameters.AddWithValue("@SecondName", entity.SecondName);
                command.Parameters.AddWithValue("@PassportData", entity.PassportData);
                command.Parameters.AddWithValue("@Phone", entity.Phone);
                command.ExecuteNonQuery();
            }
        }

        public void Delete(Passenger entity)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = connection.CreateCommand();
                command.CommandText = "DELETE FROM passengers WHERE Id = @Id";
                command.Parameters.AddWithValue("@Id", entity.Id);
                command.ExecuteNonQuery();

                // сброс идент
                int lastPassengerId = this.GetAll().ToList().LastOrDefault().Id;

                SqlCommand resetIdentCommand = new SqlCommand("DBCC CHECKIDENT ('passengers', RESEED, @newValue)", connection);
                resetIdentCommand.Parameters.AddWithValue("@newValue", lastPassengerId);
                resetIdentCommand.ExecuteNonQuery();
            }
        }

        public Passenger Get(int id)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = connection.CreateCommand();
                command.CommandText = "SELECT * FROM passengers WHERE Id = @Id";
                command.Parameters.AddWithValue("@Id", id);
                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    return new Passenger
                    {
                        Id = reader.GetInt32(0),
                        FirstName = reader.GetString(1),
                        SecondName = reader.GetString(2),
                        Phone = reader.GetString(3),
                        PassportData = reader.GetString(4),
                    };
                }
                else
                {
                    throw new Exception("Пассажир не найден.");
                }
            }
        }

        public IEnumerable<Passenger> GetAll()
        {
            var passengers = new List<Passenger>();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand cmd = connection.CreateCommand();
                cmd.CommandText = "select * from passengers";
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    passengers.Add(new Passenger
                    {
                        Id = reader.GetInt32(0),
                        FirstName = reader.GetString(1),
                        SecondName = reader.GetString(2),
                        Phone = reader.GetString(3),
                        PassportData = reader.GetString(4),
                    });
                }
            }
            return passengers;
        }

        public void Update(Passenger entity)
        {
            throw new NotImplementedException();
        }
    }
}
