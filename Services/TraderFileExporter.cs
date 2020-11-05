﻿using DayzTraderManager.Helpers;
using DayzTraderManager.Models;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;

namespace DayzTraderManager.Services
{
    public class TraderFileExporter
    {
        private readonly TraderFile _traderFile;
        private readonly string _filePath;
        private const string ENDFILE_TAG = "<FileEnd>";

        public TraderFileExporter(TraderFile traderFile, string filepath)
        {
            _traderFile = traderFile;
            _filePath = filepath;
        }


        public bool ExportFile()
        {
            try
            {
                using (var fileWriter = new StreamWriter(_filePath,false))
                {
                    fileWriter.Write(FileHeader());
                    fileWriter.Write(_traderFile.CurrencyCategory.ExportCurrencyString());
                    WriteTraderData(fileWriter, _traderFile.Traders);
                    fileWriter.Write(ENDFILE_TAG);
                }

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }

        }

        private void WriteTraderData(StreamWriter writer, ObservableCollection<Trader> traderData)
        {
            foreach (var trader in traderData)
            {
                writer.Write($"{trader.TraderTag} {trader.TraderName}\n");


                trader.TraderCategories.ToList().ForEach(cat =>
                {
                    writer.Write(cat.ExportTraderCategoryString());
                });

            }

        }

        private string FileHeader()
        {
            return $"\t\t//////////////////////////////////////////////////////////////////////////////////////////////////\n" +
                   $"\t\t//\n" +
                   $"\t\t//\t\tAuto generated file                                                                     \n" +
                   $"\t\t//\t\tGenerated Date: {DateTime.Now}                                                          \n" +
                   $"\t\t//\t\tGenerated by: DayzTraderManager - https://github.com/valvestater65/DayzTraderManager    \n" +
                   $"\t\t//\n"+
                   $"\t\t//////////////////////////////////////////////////////////////////////////////////////////////////\n\n";
                   
        }

    }
}
