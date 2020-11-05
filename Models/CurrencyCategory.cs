using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace DayzTraderManager.Models
{
    public class CurrencyCategory
    {
        public string CurrencyName { get; set; }
        public string CurrencyTag { get; set; }
        public ObservableCollection<CurrencyType> CurrencyTypes { get; set; }

        public CurrencyCategory()
        {
            CurrencyTypes = new ObservableCollection<CurrencyType>();
        }
    }
}
