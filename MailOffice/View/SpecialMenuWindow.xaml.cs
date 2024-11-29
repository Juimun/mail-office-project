using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using MailOffice.ViewModel;
using MailOfficeDataBase.DataBase;

namespace MailOffice.View;

/// <summary>
/// Логика взаимодействия для SpecialMenuWindow.xaml
/// </summary>
public partial class SpecialMenuWindow : Window {

    private SolidColorBrush _originalBackgroundBrush;
    private SolidColorBrush _originalForegroundBrush;

    public SpecialMenuWindow(MainWindowViewModel mainWindowViewModel) {
        InitializeComponent();

        // Инициализация словаря - связь Button с DataGrid
        InitializeDictionary();

        DataContext = new SpecialMenuWindowViewModel(this, new DatabaseQueries(), mainWindowViewModel);
    }

    private void TextBox_OnGotFocus(object sender, RoutedEventArgs e)
    {
        var textBox = (TextBox)sender;
        if (textBox.Text is "Введите Id пользователя" or "Введите Id почтальена")
            textBox.Text = string.Empty;
    } //TextBox_OnGotFocus

    private void TextBox_OnLostFocus(object sender, RoutedEventArgs e)
    {
        var textBox = (TextBox)sender;
        if (string.IsNullOrWhiteSpace(textBox.Text))
            textBox.Text = textBox.Name switch
            {
                "PersonIdTextBox" => "Введите Id пользователя",
                "PostmanIdTextBox" => "Введите Id почтальена",
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

    // Инициализация словаря - связь Button с DataGrid
    //  для отображения нужного DataGrid по кнопке
    private Dictionary<Button, DataGrid> dictionary = new(); 
    private void InitializeDictionary() {
        dictionary.Add(EyeVisionSubscriptions, SelectedAwaitingDataGrid);
        dictionary.Add(EyeVisionPeople, PeopleDataGrid);
        dictionary.Add(EyeVisionPostmans, PostmansDataGrid);
    } //InitializeDictionary

    private void EyeVisionButton_Click(object sender, RoutedEventArgs e) {
        var button = (Button)sender;

        if (dictionary.TryGetValue(button, out DataGrid? dataGrid)) {
            if (dataGrid != null) { 
                dataGrid.Visibility = dataGrid.Visibility == Visibility.Visible 
                    ? Visibility.Collapsed : Visibility.Visible;

                foreach (var kvp in dictionary) {
                    if (kvp.Key.Content is Image image){
                        kvp.Value.Visibility = kvp.Key == button ? dataGrid.Visibility : Visibility.Collapsed; 
                        image.Source = kvp.Value.Visibility == Visibility.Visible ?
                                       new BitmapImage(new Uri("/Assets/Menu/eye.png", UriKind.Relative)) :
                                       new BitmapImage(new Uri("/Assets/Menu/closedEye.png", UriKind.Relative));
                    }
                }  
            } //if
        } //if
    } //EyeVisionButton_Click
} //SpecialMenuWindow

