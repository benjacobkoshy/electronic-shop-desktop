using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace ElectronicShop.App.Converters
{
    public class StockStatusToBrushConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value as string switch
            {
                "Out of Stock" => new SolidColorBrush(Color.FromRgb(0xE5, 0x48, 0x4D)),
                "Low Stock" => new SolidColorBrush(Color.FromRgb(0xF5, 0x9E, 0x0B)),
                _ => new SolidColorBrush(Color.FromRgb(0x16, 0xA3, 0x4A)),
            };
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            => throw new NotSupportedException();
    }
}