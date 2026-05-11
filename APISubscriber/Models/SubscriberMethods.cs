using Npgsql;

namespace APISubscriber.Models
{
    public class SubscriberMethods
    {

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
                subscriber.SubscriptionNumber = reader["sub_subscription_number"].ToString();
                subscriber.SubscriberSocialSecutityNumber = reader["sub_social_security_number"].ToString();
                subscriber.SubscriberFirstName = reader["sub_first_name"].ToString()!;
                subscriber.SubscriberLastName = reader["sub_last_name"].ToString()!;
                subscriber.SubscriberDeliveryAddress = reader["sub_delivery_address"].ToString()!;
                subscriber.SubscriberPostalCode = reader["sub_postal_code"].ToString()!;
                subscriber.SubscriberPhoneNumber = reader["sub_phone_number"].ToString()!;
                subscriber.SubscriberCity = reader["sub_city"].ToString()!;

            }

            return subscriber;
        }

        public SubscriberDetails EditSubscriberBySubDetails(SubscriberDetails subscriber)
        {
            string connectionString = "Host=localhost;Port=5432;Database=subscribers;Username=admin;Password=isal0037";

            using NpgsqlConnection connection = new NpgsqlConnection(connectionString);

            string sqlString = "UPDATE tbl_subscribers SET sub_first_name = @firstName, sub_last_name = @lastName, sub_delivery_address = @deliveryAddress, sub_postal_code = @postalCode WHERE sub_subscription_number = @subNumber";
            using NpgsqlCommand command = new NpgsqlCommand(sqlString, connection);
            command.Parameters.AddWithValue("@firstName", subscriber.SubscriberFirstName);
            command.Parameters.AddWithValue("@lastName", subscriber.SubscriberLastName);
            command.Parameters.AddWithValue("@deliveryAddress", subscriber.SubscriberDeliveryAddress);
            command.Parameters.AddWithValue("@postalCode", subscriber.SubscriberPostalCode);
            command.Parameters.AddWithValue("@subNumber", subscriber.SubscriptionNumber);
            command.Parameters.AddWithValue("@phoneNumber", subscriber.SubscriberPhoneNumber);
            command.Parameters.AddWithValue("@city", subscriber.SubscriberCity);

            connection.Open();

            command.ExecuteNonQuery();

            return subscriber;
        }
    }
}