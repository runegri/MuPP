using System;
using System.Threading;

namespace AtB
{
    internal class DecimalConverter
    {
        private static string _decimalSeparator;
        private static string _notDecimalSeparator;

        public static double StringToDouble(string value)
        {
            if (string.IsNullOrEmpty(_decimalSeparator))
            {
                DetermineDecimalSeparator();
            }

            var formattedValue = value.Replace(_notDecimalSeparator, _decimalSeparator);
            return Convert.ToDouble(formattedValue);

        }

        public static string DoubleToString(double value)
        {
            return value.ToString().Replace(",", ".");
        }

        private static void DetermineDecimalSeparator()
        {
            _decimalSeparator = Thread.CurrentThread.CurrentCulture.NumberFormat.NumberDecimalSeparator;
            if (_decimalSeparator == ",")
            {
                _notDecimalSeparator = ".";
            }
            else
            {
                _notDecimalSeparator = ",";
            }
        }

    }
}
