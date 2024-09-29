using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xaml.Behaviors;
using System.Windows.Controls;
using OxyPlot;
using OxyPlot.Wpf;
using System.Windows;

namespace CryptocurrenciesCollector.Helpers.Behaviors
{
    public partial class PlotViewBehavior : Behavior<PlotView>
    {
        protected override void OnAttached()
        {
            base.OnAttached();
            AssociatedObject.Unloaded += OnPlotViewUnloaded;
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();
            AssociatedObject.Unloaded -= OnPlotViewUnloaded;
        }

        private void OnPlotViewUnloaded(object sender, RoutedEventArgs e)
        {
            AssociatedObject.Model = null;
        }
    }
}
