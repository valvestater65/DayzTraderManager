using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace DayzTraderManager.Models
{
    public class TraderFile
    {
        public string FilePath { get; set; }
        public CurrencyCategory CurrencyCategory { get; set; }
        public ObservableCollection<Trader> Traders { get; set; }

        public TraderFile()
        {
            CurrencyCategory = new CurrencyCategory();
            Traders = new ObservableCollection<Trader>();
        }
    }
}
