using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace DayzTraderManager.Models
{
    public class Trader
    {
        public string TraderName { get; set; }
        public string TraderTag { get; set; }
        public ObservableCollection<TraderCategory> TraderCategories { get; set; }

        public Trader()
        {
            TraderCategories = new ObservableCollection<TraderCategory>();
        }
    }
}
