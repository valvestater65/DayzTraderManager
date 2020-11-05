using DayzTraderManager.Commands;
using DayzTraderManager.Helpers;
using DayzTraderManager.Models;
using DayzTraderManager.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace DayzTraderManager.ViewModels
{
    public class MainWindowViewModel:BaseViewModel
    {
        private string _filePath;
        private TraderFile _traderData;
        //private ParameterLessCommand _parseDataFile;
        private ParameterLessCommand _saveFile;
        private string _jsonFilePath;
        private string _jsonLoadFilePath;
        private string _exportFilePath;
        private ObservableCollection<TraderItem> _flatItemList;
        private ObservableCollection<TraderItem> _itemSearchResult;
        private TraderItem _selectedItemSearch;


        private const string CURRENCYNAME_TAG = "<CurrencyName>";
        private const string CURRENCY_TAG = "<Currency>";
        private const string TRADER_TAG = "<Trader>";
        private const string CATEGORY_TAG = "<Category>";
        private const string ENDFILE_TAG = "<FileEnd>";
        private const string COMMENT_TAG = "//";


        public string FilePath { get => _filePath; set { _filePath = value; OnPropertyChanged(); } }
        public string LoadFilePath { get => _jsonLoadFilePath; set { _jsonLoadFilePath = value; OnPropertyChanged(); } }
        public string ExportFilePath { get => _exportFilePath; set { _exportFilePath = value;OnPropertyChanged(); } }
        public TraderFile TraderData { get => _traderData; set { _traderData = value; OnPropertyChanged(); } }
        
        public string SaveFilePath { get => _jsonFilePath; set { _jsonFilePath = value; OnPropertyChanged(); } }
        public ParameterLessCommand SaveFile { get => _saveFile; set { _saveFile = value; OnPropertyChanged(); } }
        public ObservableCollection<TraderItem> TraderItemList { get => _flatItemList; set { _flatItemList = value; OnPropertyChanged(); } }
        public ObservableCollection<TraderItem> ItemSearchResult { get => _itemSearchResult; set { _itemSearchResult = value; OnPropertyChanged(); } }
        public TraderItem SelectedItemSearch { get => _selectedItemSearch; set { _selectedItemSearch = value; OnPropertyChanged(); } }

        public MainWindowViewModel()
        {
            TraderData = new TraderFile();
            //ParseDataFile = new ParameterLessCommand(ParseFile);
            SaveFile = new ParameterLessCommand(GenJson);
            TraderItemList = new ObservableCollection<TraderItem>();
        }

        public void ParseFile()
        {
            try
            {
                if (string.IsNullOrEmpty(_filePath))
                {
                    throw new ArgumentException("Path can't be empty", "FilePath");
                }

                if (!File.Exists(_filePath))
                    throw new FileNotFoundException(_filePath);


                using (var fileReader = new StreamReader(_filePath))
                {
                    bool endFile = false;
                    while (!endFile)
                    {
                        string fileLine = fileReader.ReadLine().Trim();

                        if (!string.IsNullOrEmpty(fileLine) && fileLine.Contains(ENDFILE_TAG))
                        {
                            //We stop here, not reading endless empty lines
                            endFile = true;
                        }
                        else
                        {
                            ProcessFileLine(CleanLine(fileLine));
                        }
                    }
                }

                //Debug.Print("End");


            } catch (Exception ex)
            {
                throw ex;
            }

        }

        private void ProcessFileLine(string line)
        {
            try
            {
                if (!string.IsNullOrEmpty(line))
                {
                    if (line.Contains(CURRENCYNAME_TAG))
                    {
                        TraderData.CurrencyCategory.CurrencyName =
                                line.Substring(CURRENCYNAME_TAG.Length, line.Length - CURRENCYNAME_TAG.Length).Trim();
                        TraderData.CurrencyCategory.CurrencyTag = CURRENCYNAME_TAG;
                    }
                    else if (line.Contains(CURRENCY_TAG))
                    {

                        var splitValues = line.RemoveTag(CURRENCY_TAG).Split(',');

                        TraderData.CurrencyCategory.CurrencyTypes.Add(new CurrencyType
                        {
                            CurrencyTag = CURRENCY_TAG,
                            CurrencyName = splitValues[0].Trim(),
                            CurrencyValue = int.Parse(splitValues[1].Trim()),
                        });
                    }
                    else if (line.Contains(TRADER_TAG))
                    {
                        TraderData.Traders.Add(new Trader
                        {
                            TraderTag = TRADER_TAG,
                            TraderName = line.RemoveTag(TRADER_TAG).Trim()
                        });
                    }
                    else if (line.Contains(CATEGORY_TAG))
                    {
                        //As we read sequentially, we always add categories to the last created trader. 
                        var parentTrader = TraderData.Traders.Last();
                        parentTrader.TraderCategories.Add(new TraderCategory
                        {
                            CategoryTag = CATEGORY_TAG,
                            CategoryName = line.RemoveTag(CATEGORY_TAG).Trim()
                        });

                    }
                    else
                    {
                        var splitLine = line.Split(',');
                        var parentCategory = TraderData.Traders.Last().TraderCategories.Last();

                        var item = new TraderItem
                        {
                            Name = splitLine[0].Trim(),
                            ItemType = splitLine[1].Trim(),
                            BuyPrice = int.Parse(splitLine[2].Trim()),
                            SellPrice = int.Parse(splitLine[3].Trim())
                        };

                        TraderItemList.Add(item);
                        parentCategory.TraderItems.Add(item);
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.Print("CRASH");
                Debug.Print(ex.Message);
            }
        }


        public async Task ItemSearch(string searchString)
        {
            await Task.Run(() =>
            {
                ItemSearchResult = new ObservableCollection<TraderItem>(
                    _flatItemList.Where(x => x.Name.ToUpper().Contains(searchString.ToUpper ())).ToList()
                    );
            });
        }


        private string CleanLine(string line)
        {
            try
            {
                var commentIndex = line.IndexOf(COMMENT_TAG);

                if (commentIndex >= 0)
                {
                    var retLine = line.Substring(0, commentIndex).Trim();

                    if (retLine.Length > 0)
                        return retLine;
                }
                else 
                {
                    return line;
                }

                return "";
            }
            catch (Exception ex)
            {
                Debug.Print(ex.Message);
                return "";
            }
        }


        public void GenJson()
        {
            if (!string.IsNullOrEmpty(SaveFilePath))
            {

                if (File.Exists(SaveFilePath))
                    File.Delete(SaveFilePath);

                using (var fileWriter = new StreamWriter(SaveFilePath, false))
                {
                    fileWriter.Write(JsonSerializer.Serialize(TraderData));
                }
            }
        }

        public void LoadJsonFile()
        {
            try
            {
                if (!string.IsNullOrEmpty(LoadFilePath))
                {
                    using (var readStream = new StreamReader(LoadFilePath))
                    {
                        TraderData = JsonSerializer.Deserialize<TraderFile>(readStream.ReadToEnd());
                    }
                }
            }
            catch (Exception)
            {
                TraderData = null;
            }
        }

        public void ExportTraderFile()
        {
            if (string.IsNullOrEmpty(ExportFilePath))
                throw new ArgumentException("Can't be empty", "ExportFilePath");

            if (TraderData != null)
            {
                var exportManager = new TraderFileExporter(TraderData, ExportFilePath);

                if (!exportManager.ExportFile())
                    throw new Exception("Export Failed");
            }
        }
    }
}
