﻿using System.Configuration;
using System.Text;
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
using CryptocurrenciesCollector.Services;
using Microsoft.Extensions.Configuration;
using System.IO;
using Microsoft.Extensions.DependencyInjection;
using System.ComponentModel;
using CryptocurrenciesCollector.Pages;
using CryptocurrenciesCollector.Models;
using CommunityToolkit.Mvvm.Input;
using CryptocurrenciesCollector.Models.Interfaces;


namespace CryptocurrenciesCollector
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = App.Current.ServicesProvider.GetRequiredService<MainViewModel>();
        }
    }
}