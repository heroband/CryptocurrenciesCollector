﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
using CryptocurrenciesCollector.ViewModels;

namespace CryptocurrenciesCollector.Pages
{
    public partial class ConvertPage : Page
    {
        public ConvertPage(MainViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;
        }

        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex(@"^\d+(\.\d*)?$");
            e.Handled = !regex.IsMatch(((TextBox)sender).Text + e.Text);
        }

    }
}
