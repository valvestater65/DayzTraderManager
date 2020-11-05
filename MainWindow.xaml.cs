using DayzTraderManager.ViewModels;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace DayzTraderManager
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        private readonly MainWindowViewModel _viewModel;
        public MainWindow()
        {
            _viewModel = new MainWindowViewModel();
            InitializeComponent();
            DataContext = _viewModel;
        }

        private void btnLoadFile_Click(object sender, RoutedEventArgs e)
        {
            var fileDialog = new OpenFileDialog();

            if (fileDialog.ShowDialog(this).Value)
            {
                _viewModel.FilePath = fileDialog.FileName;
                _viewModel.ParseFile();
            }
        }

        private void btnSaveFile_Click(object sender, RoutedEventArgs e)
        {
            var saveFileDialog = new SaveFileDialog();

            saveFileDialog.Filter = "JSON | *.json";
            

            if (saveFileDialog.ShowDialog(this).Value)
            {
                _viewModel.SaveFilePath = saveFileDialog.FileName;
                _viewModel.GenJson();
            }
        }

        private async void txtItemSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (txtItemSearch.Text.Length > 3)
            {
                await _viewModel.ItemSearch(txtItemSearch.Text);
            }
        }

        private void btnLoadSavedFile_Click(object sender, RoutedEventArgs e)
        {
            var openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "JSON|*.json";

            if (openFileDialog.ShowDialog(this).Value)
            {
                _viewModel.LoadFilePath = openFileDialog.FileName;
                _viewModel.LoadJsonFile();
            }
        }

        private void btnExportFile_Click(object sender, RoutedEventArgs e)
        {
            var saveFileDialog = new SaveFileDialog();
            saveFileDialog.FileName = "TraderConfig.txt";
            saveFileDialog.Filter = "Text File|*.txt";

            if (saveFileDialog.ShowDialog().Value)
            { 

            }
        }
    }
}
