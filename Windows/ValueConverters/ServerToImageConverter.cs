using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace Windows
{
    public class ServerToImageConverter : BaseValueConverter<ServerToImageConverter>
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var serverFlag = (string)value;

            if (serverFlag == null)
            {
                return null;
            }

            var filename = $"Images/flags_iso/32/{serverFlag}";
            return new BitmapImage(new Uri($"pack://application:,,,/{filename}"));


        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
