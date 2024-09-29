using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using Microsoft.Xaml.Behaviors;

namespace CryptocurrenciesCollector.Helpers.Behaviors
{
    public partial class ExchangeTextBoxValidationBehavior : Behavior<TextBox>
    {
        protected override void OnAttached()
        {
            base.OnAttached();
            AssociatedObject.PreviewTextInput += OnPreviewTextInput;
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();
            AssociatedObject.PreviewTextInput -= OnPreviewTextInput;
        }
        private void OnPreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = ExchangeTextBoxValidation();
            e.Handled = !regex.IsMatch(((TextBox)sender).Text + e.Text);
        }

        [GeneratedRegex(@"^(0(\.\d*)?|[1-9]\d*(\.\d*)?)$")]
        private static partial Regex ExchangeTextBoxValidation();
    }
}
