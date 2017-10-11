using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Data;
using System.Globalization;

namespace ESnail.Utilities.Data
{

    [ValueConversion(typeof(UInt32), typeof(String))]
    public class HEXU32Converter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            UInt32 tValue = (UInt32)value;
            return tValue.ToString("X8");
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            UInt32 tResult = 0;
            String tHEX = value as String;
            if (UInt32.TryParse(tHEX, NumberStyles.HexNumber, null, out tResult))
            {
                return tResult;
            }
            return value;
        }
    }

    [ValueConversion(typeof(UInt16), typeof(String))]
    public class HEXU16Converter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            UInt16 tValue = (UInt16)value;
            return tValue.ToString("X4");
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            UInt16 tResult = 0;
            String tHEX = value as String;
            if (UInt16.TryParse(tHEX, NumberStyles.HexNumber, null, out tResult))
            {
                return tResult;
            }
            return value;
        }
    }

    [ValueConversion(typeof(Byte), typeof(String))]
    public class HEXU8Converter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            Byte tValue = (Byte)value;
            return tValue.ToString("X4");
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            Byte tResult = 0;
            String tHEX = value as String;
            if (Byte.TryParse(tHEX, NumberStyles.HexNumber, null, out tResult))
            {
                return tResult;
            }
            return value;
        }
    }

    [ValueConversion(typeof(Int32), typeof(Boolean))]
    public class NoneZeroConvert : IValueConverter
    {
        public Object Convert(Object value, Type targetType, Object parameter, CultureInfo culture)
        {
            Int32 tValue = (Int32)value;
            return tValue > 0;
        }

        public Object ConvertBack(Object value, Type targetType, Object parameter, CultureInfo culture)
        {
            return Binding.DoNothing;
        }
    }

}
