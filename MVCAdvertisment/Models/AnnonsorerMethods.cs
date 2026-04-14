using Npgsql;
namespace MVCAdvertisment.Models
{
    public class AnnonsorerMethods
    {
        public AnnonsorerMethods() { }

        public int InsertAdvertiser(AnnonsorerDetails annonsor)
        {
            string connectionString = "Host=localhost;Port=5432;Database=advertisment;Username=admin;Password=isal0037";
            NpgsqlConnection connection = new NpgsqlConnection(connectionString);

            string sqlString = "INSERT INTO tbl_annonsorer (adv_name, adv_number, adv_phone_number, adv_delivery_address, adv_postal_code, adv_city, adv_billing_address) VALUES (@advName, @advNumber, @advPhoneNumber, @advDeliveryAddress, @advPostalCode, @advCity, @advBillingAddress)";
            NpgsqlCommand command = new NpgsqlCommand(sqlString, connection);
            command.Parameters.AddWithValue("@advName", annonsor.AdvName);
            command.Parameters.AddWithValue("@advNumber", annonsor.AdvNumber);
            command.Parameters.AddWithValue("@advPhoneNumber", annonsor.AdvPhoneNumber);
            command.Parameters.AddWithValue("@advDeliveryAddress", annonsor.AdvDeliveryAddress);
            command.Parameters.AddWithValue("@advPostalCode", annonsor.AdvPostalCode);
            command.Parameters.AddWithValue("@advCity", annonsor.AdvCity);
            command.Parameters.AddWithValue("@advBillingAddress", annonsor.AdvBillingAddress);

            try
            {
                connection.Open();
                int rows = command.ExecuteNonQuery();
                return rows;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error inserting advertiser: " + ex.Message);
            }
            finally
            {
                connection.Close();
            }
            return 0; 
        }

        public AnnonsorerDetails? GetAdvertiserByAdvNumber(string advNumber)
        {
            if(string.IsNullOrEmpty(advNumber))
            {
                Console.WriteLine("Advertiser number is null or empty");
                return null;
            }
            
            string connectionString = "Host=localhost;Port=5432;Database=advertisment;Username=admin;Password=isal0037";
            using NpgsqlConnection connection = new NpgsqlConnection(connectionString);

            string sqlString = "SELECT * FROM tbl_annonsorer WHERE adv_number = @advNumber";
            using NpgsqlCommand command = new NpgsqlCommand(sqlString, connection);
            command.Parameters.AddWithValue("@advNumber", advNumber);

            connection.Open();

            AnnonsorerDetails annonsor = new AnnonsorerDetails();
            using NpgsqlDataReader reader = command.ExecuteReader();

            if (reader.Read())
            {
                return new AnnonsorerDetails
                {
                    AdvId = Convert.ToInt32(reader["adv_id"]),
                    AdvName = reader["adv_name"].ToString(),
                    AdvNumber = reader["adv_number"].ToString(),
                    AdvPhoneNumber = reader["adv_phone_number"].ToString(),
                    AdvDeliveryAddress = reader["adv_delivery_address"].ToString(),
                    AdvPostalCode = reader["adv_postal_code"].ToString(),
                    AdvCity = reader["adv_city"].ToString(),
                    AdvBillingAddress = reader["adv_billing_address"].ToString()
                };
            }

            return null;
        }
        
    }
}