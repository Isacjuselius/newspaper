using Npgsql;

namespace APISubscriber.Models
{
    public class SubscriberMethods
    {
        public List<SubscriberDetails> GetAllSubscribers()
        {
            string connectionString = "Host=localhost;Port=5432;Database=subscribers;Username=admin;Password=isal0037";

            List<SubscriberDetails> subscribers = new List<SubscriberDetails>();

            using NpgsqlConnection connection = new NpgsqlConnection(connectionString);

            string sqlString = "SELECT * FROM tbl_subscribers";
            using NpgsqlCommand command = new NpgsqlCommand(sqlString, connection);

            connection.Open();

            using NpgsqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                SubscriberDetails subscriber = new SubscriberDetails();
                subscriber.SubscriberFirstName = reader["sub_first_name"].ToString()!;
                subscriber.SubscriberLastName = reader["sub_last_name"].ToString()!;
                subscriber.SubscriberDeliveryAddress = reader["sub_delivery_address"].ToString()!;
                subscriber.SubscriberPostalCode = reader["sub_postal_code"].ToString()!;

                subscribers.Add(subscriber);
            }

            return subscribers;
        }

        public SubscriberDetails GetSubscriberBySubNumber(string subNumber)
        {
            string connectionString = "Host=localhost;Port=5432;Database=subscribers;Username=admin;Password=isal0037";

            using NpgsqlConnection connection = new NpgsqlConnection(connectionString);

            string sqlString = "SELECT * FROM tbl_subscribers WHERE sub_subscription_number = @subNumber";
            using NpgsqlCommand command = new NpgsqlCommand(sqlString, connection);
            command.Parameters.AddWithValue("@subNumber", subNumber);

            connection.Open();

            SubscriberDetails subscriber = new SubscriberDetails();
            using NpgsqlDataReader reader = command.ExecuteReader();
            
            if (reader.Read())
            {
                subscriber.SubscriberId = Convert.ToInt32(reader["subscriber_id"]);
                subscriber.SubsciptionNumber = reader["sub_subscription_number"].ToString();
                subscriber.SubscriberSocialSecutityNumber = reader["sub_social_security_number"].ToString();
                subscriber.SubscriberFirstName = reader["sub_first_name"].ToString()!;
                subscriber.SubscriberLastName = reader["sub_last_name"].ToString()!;
                subscriber.SubscriberDeliveryAddress = reader["sub_delivery_address"].ToString()!;
                subscriber.SubscriberPostalCode = reader["sub_postal_code"].ToString()!;
            }

            return subscriber;
        }
    }
}