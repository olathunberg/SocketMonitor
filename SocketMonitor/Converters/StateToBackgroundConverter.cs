using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace TTech.SocketMonitor.Converters
{
    public class StateToBackgroundConverter : IValueConverter
    {
        public object Convert(object value, Type targetType,
          object parameter, CultureInfo culture)
        {
            var color = Colors.Transparent;
            if (value != null && value is Models.SocketState state)
            {
                switch (state)
                {
                    case Models.SocketState.None:
                        color = Colors.Red;
                        break;
                    case Models.SocketState.New:
                        color = Colors.LightGreen;
                        break;
                    case Models.SocketState.Closed:
                        color = Colors.PaleVioletRed;
                        break;
                    case Models.SocketState.Changed:
                        color = Colors.LightYellow;
                        break;
                    case Models.SocketState.Steady:
                        color = Colors.Transparent;
                        break;
                }
            }

            return new SolidColorBrush(color);
        }

        public object ConvertBack(object value, Type targetType,
            object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

    }
}
