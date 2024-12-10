using MailOffice.ViewModel;
using MailOfficeDataBase.DataBase;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace MailOffice.View;

/// <summary>
/// Логика взаимодействия для AdditionalInformationWindow.xaml
/// </summary>
public partial class AdditionalInformationWindow : Window {

    private SolidColorBrush _originalBackgroundBrush;
    private SolidColorBrush _originalForegroundBrush;

    public AdditionalInformationWindow(BuySubscriptionWindowViewModel buySubscriptionWindowViewModel) {
        InitializeComponent();

        DataContext = new AdditionalInformationWindowViewModel(this, new DatabaseQueries(), buySubscriptionWindowViewModel);
    }

    private void TextBox_OnGotFocus(object sender, RoutedEventArgs e)
    {
        var textBox = (TextBox)sender;
        if (textBox.Text is "Введите ваше имя" 
            or "Введите вашу фамилию" 
            or "Введите свое отчество" 
            or "Введите наименование улицы"
            or "Введите номер дома")
            textBox.Text = string.Empty;
    } //TextBox_OnGotFocus

    private void TextBox_OnLostFocus(object sender, RoutedEventArgs e)
    {
        var textBox = (TextBox)sender;
        if (string.IsNullOrWhiteSpace(textBox.Text))
            textBox.Text = textBox.Name switch
            {
                "FirstNameTextBox" => "Введите ваше имя",
                "SecondNameTextBox" => "Введите вашу фамилию",
                "PatronymicTextBox" => "Введите свое отчество",
                "StreetTextBox" => "Введите наименование улицы",
                "HouseNumberTextBox" => "Введите номер дома",
                _ => textBox.Text
            };
    } //TextBox_OnLostFocus

    private void UIElement_OnMouseEnter(object sender, MouseEventArgs mouseEventArgs)
    {
        Button button = (Button)sender;
        _originalBackgroundBrush = (SolidColorBrush)button.Background;
        _originalForegroundBrush = (SolidColorBrush)button.Foreground;

        button.Foreground = Brushes.Black;
    }

    private void UIElement_OnMouseLeave(object sender, MouseEventArgs mouseEventArgs)
    {
        Button button = (Button)sender;

        // Восстанавливаем исходные цвета
        button.Background = _originalBackgroundBrush;
        button.Foreground = _originalForegroundBrush;
    }

}
