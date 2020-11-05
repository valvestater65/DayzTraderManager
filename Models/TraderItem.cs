using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace DayzTraderManager.Models
{
    public class TraderItem
    {
        public string Name { get; set; }
        public double SellPrice { get; set; }
        public double BuyPrice { get; set; }
        public string ItemType { get; set; }
        
        [JsonIgnore]
        public double SellMargin { get 
            {
                try
                {
                    return Math.Abs(Math.Round(BuyPrice / SellPrice,2) *100);
                }
                catch (Exception)
                {
                    return 0.0;
                }
            } 
        }

        public TraderItem()
        {
            SellPrice = 0;
            BuyPrice = 0;
        }
    }
}
