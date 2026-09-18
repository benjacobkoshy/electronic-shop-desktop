using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace ElectronicShop.App.Converters
{
    public class ZeroToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var isZero = value is int i && i == 0;
            var invert = parameter as string == "Inverse";

            if (invert) isZero = !isZero;
            return isZero ? Visibility.Collapsed : Visibility.Visible;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            => throw new NotSupportedException();
    }
}