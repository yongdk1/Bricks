using System;
using System.Collections.Generic;
using System.Linq;
using System.Globalization;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.Design;
using System.ComponentModel;
//For using SQL Database
using MySql.Data.MySqlClient;
using Microsoft.Extensions.Configuration;

namespace BricksMeatballs.Models
{
    public abstract class ChartViewModel
    {
        public DateTime saleTime { get; set; }
        public abstract int dataPoint { get; set; }
    }

    public class AvgViewModel : ChartViewModel
    {
        public List <int> priceForMonth { get; set; }
        public override int dataPoint { get; set; }
    }

    public class ChartDataAcess
    {
        public static List<AvgViewModel> GetAvgPriceList(String district)
        {
            //string con = 
            //string cmd = "SELECT contractDate FROM bricks WHERE district = 'blahblahblah' ORDER BY contractDate";

            var datalist = new List<AvgViewModel>();

            var dataDictionary = GetAllPrices(district);
            foreach(var month in dataDictionary)
            {
                datalist.Add(new AvgViewModel { saleTime = DateTime.ParseExact(month.Key, "MMyy", CultureInfo.InvariantCulture), priceForMonth = month.Value, dataPoint = (int)month.Value.Average()});
            }
            

            return datalist;
        }

        private static Dictionary<string,List<int>> GetAllPrices(String district)
        {
            //To store the values in a dictionary with key as the date and value as a dynamically increasing list of prices
            var dataDictionary = new Dictionary<string, List<int>>();

            using (MySqlConnection conn = new MySqlConnection("server=localhost;port=3306;database=bricks;user=root;password=password;"))
            {
                conn.Open();
                String stringCmd = String.Format(@"select contractDate, price from bricks.pmiresidenceresult where district = {0}
order by substring(contractDate,3,2),substring(contractDate,1,2)", district);
                MySqlCommand cmd = new MySqlCommand(stringCmd, conn);

                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        string contractDate = reader["contractDate"].ToString();
                        if (!dataDictionary.ContainsKey(contractDate))
                        {
                            dataDictionary.Add(contractDate, new List<int>() { Convert.ToInt32(reader["price"]) });
                        }
                        else
                        {
                            dataDictionary[contractDate].Add(Convert.ToInt32(reader["price"]));
                        }
                    }
                }
            }


            return dataDictionary;
        }


    }

    


}
