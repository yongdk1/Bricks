using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.Design;
using System.ComponentModel;
using Microsoft.Extensions.Configuration;
using System.Data;

/// <summary>
/// Author: RuiYang
/// Modified By: Pei Yan, Chaoshan
/// The chart model class
/// Contains the base classes and inherited classes and ChartDataAccess for retrieving SQLDatabase
/// </summary>
namespace BricksMeatballs.Models
{
    /// <summary>
    /// All chart models inherit from this class
    /// </summary>
    public class ChartViewModel
    {
        public string saleTime { get; set; }
        public string averagePrice { get; set; }
        public string movingAveragePrice { get; set; }
    }

    /// <summary>
    /// Retrieve data to plot graph
    /// </summary>
    public class ChartDataAcess
    {
        public IConfiguration Configuration { get; }

        public ChartDataAcess(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        ///<summary>
        ///Uses the dictionary returned from GetAllPrices to create a list of chartmodels with appropriate values
        /// </summary>
        ///<returns>
        ///List of chart models with calculated average for the month and moving average across 3 months
        /// </returns>
        public List<ChartViewModel> GetAvgPriceList(string Location)
        {
            PullData db = new PullData(Configuration);

            string Query = "SELECT Sum(price) FROM bricks.pmiresidenceresult ";
            if (Location != "" && Location != null)
            {
                Query = Query + " Where street='" + Location + "'";
            }

            DataTable dtPriceSum = db.LoadData(Query, "PriceSum");


            string resultQuery = "SELECT contractDate,FORMAT(((Sum(price)/" + dtPriceSum.Rows[0][0].ToString() + ")*100),6) AveragePrice FROM bricks.pmiresidenceresult ";
            if (Location != "" && Location != null)
            {
                resultQuery = resultQuery + " Where street='" + Location + "'";
            }
            resultQuery = resultQuery + @" Group by contractDate order by STR_TO_DATE(concat('01,', left(contractDate, 2),',20',right(contractDate, 2)),'%d,%m,%Y')";

            DataTable dtresultQuery = db.LoadData(resultQuery, "resultQuery");
            for (int i = 0; i < dtresultQuery.Rows.Count; i++)
            {
                string strMonth = dtresultQuery.Rows[i]["contractDate"].ToString().Substring(0, 2);
                string strYear = "20" + dtresultQuery.Rows[i]["contractDate"].ToString().Substring(2, 2);
                dtresultQuery.Rows[i]["contractDate"] = strMonth + "-" + strYear;
            }

            var datalist = new List<ChartViewModel>();
            datalist.Add(new ChartViewModel { saleTime = dtresultQuery.Rows[0][0].ToString(), averagePrice = dtresultQuery.Rows[0][1].ToString(), movingAveragePrice = dtresultQuery.Rows[0][1].ToString() });
            double sumOfMonths = Double.Parse(dtresultQuery.Rows[0][1].ToString()) + Double.Parse(dtresultQuery.Rows[1][1].ToString());
            double movingAverage = sumOfMonths / 2;
            datalist.Add(new ChartViewModel { saleTime = dtresultQuery.Rows[1][0].ToString(), averagePrice = dtresultQuery.Rows[1][1].ToString(), movingAveragePrice = movingAverage.ToString("N6") });
            for (int i = 2; i < dtresultQuery.Rows.Count; i++)
            {
                sumOfMonths = Double.Parse(dtresultQuery.Rows[i - 2][1].ToString()) + Double.Parse(dtresultQuery.Rows[i - 1][1].ToString()) + Double.Parse(dtresultQuery.Rows[i][1].ToString());
                movingAverage = sumOfMonths / 3;
                datalist.Add(new ChartViewModel { saleTime = dtresultQuery.Rows[i][0].ToString(), averagePrice = dtresultQuery.Rows[i][1].ToString(), movingAveragePrice = movingAverage.ToString("N6") });
            }
            return datalist;
        }
    }
}
