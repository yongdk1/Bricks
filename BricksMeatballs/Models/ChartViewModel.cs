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
/// <summary>
/// The chart model class
/// Contains the base classes and inherited classes and ChartDataAccess for retrieving SQLDatabase
/// </summary>
namespace BricksMeatballs.Models
{
    public abstract class ChartViewModel
    {
        /// <summary>
        /// All chart models inherit from this class
        /// </summary>
        public DateTime saleTime { get; set; }
        public abstract int dataPoint { get; set; }
    }

    public class AvgViewModel : ChartViewModel
    {
        /// <summary>
        /// Inherits with additional attributes.
        /// list of int priceForMonth for values within a 3 month range for calculating moving average
        /// int avgMonth for average price for the month
        /// </summary>
        public List <int> priceForMonth { get; set; }
        public int avgMonth { get; set; }
        public override int dataPoint { get; set; }
    }

    public class ChartDataAcess
    {
        public static List<AvgViewModel> GetAvgPriceList(String district)
        {
            ///<summary>
            ///Uses the dictionary returned from GetAllPrices to create a list of chartmodels with appropriate values
            /// </summary>
           
            ///<returns>
            ///List of chart models with calculated average for the month and moving average across 3 months
            /// </returns>

            var datalist = new List<AvgViewModel>();

            var dataDictionary = GetAllPrices(district);
            foreach(var month in dataDictionary)
            {
                if (month.Value.Count >= 20) datalist.Add(new AvgViewModel { saleTime = DateTime.ParseExact(month.Key, "MMyy", CultureInfo.InvariantCulture), priceForMonth = month.Value,avgMonth = (int)month.Value.Average()});
            }
            
            for(int i = 0; i <datalist.Count; i++)
            {
                datalist[i].dataPoint = (datalist[i - 1 < 0 ? i : i - 1].avgMonth + datalist[i].avgMonth + datalist[i + 1 >= datalist.Count ? i : i + 1].avgMonth) / 3;
            }

            return datalist;
        }

        private static Dictionary<string,List<int>> GetAllPrices(String district)
        {
            //To store the values in a dictionary with key as the date and value as a dynamically increasing list of prices
            ///<summary>
            ///retrieve all values from sqldatabase and store it according to saletime
            /// </summary>
            /// <returns>
            ///Dictionary for easy retrieval in other methods
            /// </returns>
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
