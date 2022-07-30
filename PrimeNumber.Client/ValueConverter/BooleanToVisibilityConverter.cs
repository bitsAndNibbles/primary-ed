using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace PrimeNumber.Client.ValueConverter;

public class BooleanToVisibilityConverter : IValueConverter
{
    public Visibility ValueWhenTrue { get; set; } = Visibility.Visible;
    public Visibility ValueWhenFalse { get; set; } = Visibility.Collapsed;

    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        bool boolValue = (bool)value;

        return boolValue ? ValueWhenTrue : ValueWhenFalse;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }

}
