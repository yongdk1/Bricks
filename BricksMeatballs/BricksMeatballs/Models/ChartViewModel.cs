using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.Design;
using System.ComponentModel;

namespace BricksMeatballs.Models
{
    public class ChartViewModel
    {
        public int saleTime { get; set; }
        public int averagePrice { get; set; }
    }

    public class ChartDataAcess
    {
        public static List<ChartViewModel> GetAvgPriceList()
        {
            var datalist = new List<ChartViewModel>();
            datalist.Add(new ChartViewModel { saleTime = 06, averagePrice = 100000 });
            datalist.Add(new ChartViewModel { saleTime = 07, averagePrice = 300000 });
            datalist.Add(new ChartViewModel { saleTime = 08, averagePrice = 500000 });
            datalist.Add(new ChartViewModel { saleTime = 09, averagePrice = 400000 });
            datalist.Add(new ChartViewModel { saleTime = 10, averagePrice = 200000 });

            return datalist;
        }
    }
}
