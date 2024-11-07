using MailOffice.ViewModel.Queries;
using MailOfficeDataBase.DataBase;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;


namespace MailOffice.View.Queries;

/// <summary>
/// Логика взаимодействия для Query2Window.xaml
/// </summary>
public partial class Query2Window : Window {

    private DatabaseQueries _data;

    private SolidColorBrush _originalBackgroundBrush;
    private SolidColorBrush _originalForegroundBrush;

    public Query2Window() {
        InitializeComponent();

        DataContext = new Query2ViewModel(this);

        _data = new DatabaseQueries();
        StreetComboBox.ItemsSource = _data.GetAllStreets();
    }

    // Событие для выбора номера дома только по выбранной улице
    private void StreetComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e) {
        if (StreetComboBox.SelectedItem != null) 
            HouseNumberComboBox.ItemsSource = _data.GetHouseNumbersByStreet(StreetComboBox.SelectedItem.ToString());
    } //StreetComboBox_SelectionChanged

    private void UIElement_OnMouseEnter(object sender,MouseEventArgs mouseEventArgs)
    {
        Button button = (Button)sender;
        _originalBackgroundBrush = (SolidColorBrush)button.Background;
        _originalForegroundBrush = (SolidColorBrush)button.Foreground;

        button.Foreground = Brushes.Black;
    }

    private void UIElement_OnMouseLeave(object sender, MouseEventArgs e)
    {
        Button button = (Button)sender;

        // Восстанавливаем исходные цвета
        button.Background = _originalBackgroundBrush;
        button.Foreground = _originalForegroundBrush;
    }

    private void Button_IsEnabledChanged(object sender, DependencyPropertyChangedEventArgs e)
    {
        var button = (Button)sender;
        if (!button.IsEnabled)
        {
            _originalBackgroundBrush = (SolidColorBrush)button.Background;
            _originalForegroundBrush = (SolidColorBrush)button.Foreground;

            button.Foreground = Brushes.Black;
        }
        else
        {
            // Восстанавливаем исходные цвета
            button.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#235ecf"));
            button.Foreground = _originalForegroundBrush;
        }
    }

} //Query2Window