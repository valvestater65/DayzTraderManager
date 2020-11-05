using DayzTraderManager.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;

namespace DayzTraderManager.Helpers
{
    public static class TraderDataHelpers
    {
        public static string ExportCurrencyString(this CurrencyCategory currencyCategory)
        {
            try
            {
                string retString = $"{currencyCategory.CurrencyTag} {currencyCategory.CurrencyName} \n";

                foreach (var currency in currencyCategory.CurrencyTypes)
                {
                    retString += $"\t{currency.CurrencyTag} {currency.CurrencyName},\t\t{currency.CurrencyValue}\n";
                }

                return retString + "\n";
            }
            catch (Exception)
            {
                return "";
            }
        }

        public static string ExportTraderCategoryString(this TraderCategory traderCategory)
        {

            var retString = $"\t{traderCategory.CategoryTag} {traderCategory.CategoryName}\n";

            traderCategory.TraderItems.ToList().ForEach(item => {
                retString += $"\t\t{item.Name},\t\t\t{item.ItemType},\t\t{item.BuyPrice},\t\t{item.SellPrice}\n";
            });

            return retString + "\n";
        }
    }
}
