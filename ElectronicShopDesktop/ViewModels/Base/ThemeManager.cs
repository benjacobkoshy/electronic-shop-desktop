using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ElectronicShop.App.ViewModels.Base
{
    public static class ThemeManager
    {
        public enum AppTheme { Light, Dark }

        public static AppTheme Current { get; private set; } = AppTheme.Light;

        public static void ApplyTheme(AppTheme theme)
        {
            var dictionaries = Application.Current.Resources.MergedDictionaries;

            var newTheme = new ResourceDictionary
            {
                Source = new Uri($"/ElectronicShop.App;component/Resources/Themes/{theme}Theme.xaml", UriKind.Relative)
            };

            var existing = dictionaries.FirstOrDefault(d =>
                d.Source != null && d.Source.OriginalString.Contains("Theme.xaml"));
            if (existing != null) dictionaries.Remove(existing);

            dictionaries.Add(newTheme);
            Current = theme;
        }

        public static void Toggle() =>
            ApplyTheme(Current == AppTheme.Light ? AppTheme.Dark : AppTheme.Light);
    }
}
