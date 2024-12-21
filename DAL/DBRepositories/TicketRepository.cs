using DAL.Entities;
using DAL.Interfaces;
using System.Data.SqlClient;

namespace DAL.DBRepositories
{
    public class TicketRepository : ITicketRepository
    {
        private string connectionString;
        //private SqlConnection connection;

        public TicketRepository(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public void Add(Ticket entity)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = connection.CreateCommand();
                command.CommandText = "insert into tickets (Source, Destination, StartTime, ArrivalTime, WagonNumber, PlaceNumber) values " +
                                      "(@Source, @Destination, @StartTime, @ArrivalTime, @WagonNumber, @PlaceNumber)";
                command.Parameters.AddWithValue("@Source", entity.Source);
                command.Parameters.AddWithValue("@Destination", entity.Destination);
                command.Parameters.AddWithValue("@StartTime", entity.StartTime);
                command.Parameters.AddWithValue("@ArrivalTime", entity.ArrivalTime);
                command.Parameters.AddWithValue("@WagonNumber", entity.WagonNumber);
                command.Parameters.AddWithValue("@PlaceNumber", entity.PlaceNumber);
                command.ExecuteNonQuery();
            }
        }

        public void Delete(Ticket entity)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = connection.CreateCommand();
                command.CommandText = "DELETE FROM tickets WHERE Id = @Id";
                command.Parameters.AddWithValue("@Id", entity.Id);
                command.ExecuteNonQuery();

                // сброс идент
                int lastTicketId = this.GetAll().ToList().LastOrDefault().Id;

                SqlCommand resetIdentCommand = new SqlCommand("DBCC CHECKIDENT ('tickets', RESEED, @newValue)", connection);
                resetIdentCommand.Parameters.AddWithValue("@newValue", lastTicketId);
                resetIdentCommand.ExecuteNonQuery();
            }
        }

        public Ticket Get(int id)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = connection.CreateCommand();
                command.CommandText = "SELECT * FROM tickets WHERE Id = @Id";
                command.Parameters.AddWithValue("@Id", id);
                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    return new Ticket
                    {
                        Id = reader.GetInt32(0),
                        Source = reader.GetString(1),
                        Destination = reader.GetString(2),
                        StartTime = reader.GetDateTime(3),
                        ArrivalTime = reader.GetDateTime(4),
                        WagonNumber = reader.GetInt32(5),
                        PlaceNumber = reader.GetInt32(6)
                    };
                }
                else
                {
                    throw new Exception("Билет не найден.");
                }
            }
        }

        public IEnumerable<Ticket> GetAll()
        {
            var tickets = new List<Ticket>();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand cmd = connection.CreateCommand();
                cmd.CommandText = "select * from tickets";
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    tickets.Add(new Ticket
                    {
                        Id = reader.GetInt32(0),
                        Source = reader.GetString(1),
                        Destination = reader.GetString(2),
                        StartTime = reader.GetDateTime(3),
                        ArrivalTime = reader.GetDateTime(4),
                        WagonNumber = reader.GetInt32(5),
                        PlaceNumber = reader.GetInt32(6)
                    });
                }
            }
            return tickets;
        }

        public void Update(Ticket entity)
        {
            throw new NotImplementedException();
        }
    }
}
