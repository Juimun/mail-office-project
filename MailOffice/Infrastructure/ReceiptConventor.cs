using System.Globalization;
using System.Text;
using System.Windows.Data;

namespace MailOffice.Infrastructure;

// Конвентер значений в DataGrid
public class ReceiptConventor : IValueConverter {

    // Принимает объект данных и преобразует его в значение
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
        return value switch {
            List<string> publicationsName => string.Join("\n", publicationsName),
            List<int> durations => string.Join("\n", durations),
            _ => string.Empty
        };
    } //Convert

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
        throw new NotImplementedException();
    }

} //ReceiptConventor
