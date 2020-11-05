using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace DayzTraderManager.Models
{
    public class TraderCategory
    {
        public string CategoryName { get; set; }
        public string CategoryTag { get; set; }
        public ObservableCollection<TraderItem> TraderItems { get; set; }

        public TraderCategory()
        {
            TraderItems = new ObservableCollection<TraderItem>();
        }
    }
}
