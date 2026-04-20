using Npgsql;
namespace MVCAdvertisment.Models
{
    public class AdsMethods
    {
        public AdsMethods() { }

        public int InsertAd(AdsDetails ad)
        {
            string connectionString = "Host=localhost;Port=5432;Database=advertisment;Username=admin;Password=isal0037";
            NpgsqlConnection connection = new NpgsqlConnection(connectionString);

            string sqlString = "INSERT INTO tbl_ads (ad_title, ad_description, ad_item_price, ad_price, adv_id) VALUES (@adTitle, @adDescription, @adItemPrice, @adPrice, @advId)";
            NpgsqlCommand command = new NpgsqlCommand(sqlString, connection);
            command.Parameters.AddWithValue("@adTitle", ad.AdTitle);
            command.Parameters.AddWithValue("@adDescription", ad.AdDescription);
            command.Parameters.AddWithValue("@adItemPrice", ad.AdItemPrice);
            command.Parameters.AddWithValue("@adPrice", ad.AdPrice);
            command.Parameters.AddWithValue("@advId", ad.AdvId);

            try
            {
                connection.Open();
                int rows = command.ExecuteNonQuery();
                return rows;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error inserting ad: " + ex.Message);
            }
            finally
            {
                connection.Close();
            }
            return 0; 
        }
    }
}