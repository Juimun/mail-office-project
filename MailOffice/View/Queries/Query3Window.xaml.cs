using MailOffice.ViewModel.Queries;
using MailOfficeDataBase.DataBase;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace MailOffice.View.Queries;

/// <summary>
/// Логика взаимодействия для Query3Window.xaml
/// </summary>
public partial class Query3Window : Window {

    private DatabaseQueries _data;

    private SolidColorBrush _originalBackgroundBrush;
    private SolidColorBrush _originalForegroundBrush;

    public Query3Window()
    {
        InitializeComponent();

        DataContext = new Query3ViewModel(this);

        _data = new DatabaseQueries();
        NameComboBox.ItemsSource = _data.GetAllNames(); 
    }

    private void TextBox_OnGotFocus(object sender, RoutedEventArgs e)
    {
        var textBox = (TextBox)sender;
        if (textBox.Text is "Введите имя" or "Введите фамилию" or "Введите отчество")
            textBox.Text = string.Empty;
    } //TextBox_OnGotFocus

    private void TextBox_OnLostFocus(object sender, RoutedEventArgs e)
    {
        var textBox = (TextBox)sender;
        if (string.IsNullOrWhiteSpace(textBox.Text))
            textBox.Text = textBox.Name switch
            {
                "NameTextBox" => "Введите имя",
                "SecondNameTextBox" => "Введите фамилию",
                "PatronymicTextBox" => "Введите отчество",
                _ => textBox.Text
            };
    } //TextBox_OnLostFocus

    private void UIElement_OnMouseEnter(object sender, MouseEventArgs e)
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

    private void Button_IsEnabledChanged(object sender, DependencyPropertyChangedEventArgs e) {
        var button = (Button)sender;
        if (!button.IsEnabled) {
            _originalBackgroundBrush = (SolidColorBrush)button.Background;
            _originalForegroundBrush = (SolidColorBrush)button.Foreground;

            button.Foreground = Brushes.Black;
        }
        else {
            // Восстанавливаем исходные цвета
            button.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#235ecf"));
            button.Foreground = _originalForegroundBrush;
        }
    }

    // Событие для выбора фамилии по выбранному имени
    private void FirstNameComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e) {
        if (NameComboBox.SelectedItem != null) {
            SecondNameComboBox.ItemsSource = _data.GetSurnamesByFirstName(NameComboBox.SelectedItem.ToString());
            SecondNameComboBox.IsEnabled = true;
        } //if
    } //FirstNameComboBox_SelectionChanged 

    // Событие для выбора фамилии по выбранному имени
    private void SecondNameComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e) {
        if (SecondNameComboBox.SelectedItem != null) {
            PatronymicComboBox.ItemsSource = _data.GetPatronymicsBySecondName(NameComboBox.SelectedItem.ToString(), SecondNameComboBox.SelectedItem.ToString()); 
            PatronymicComboBox.IsEnabled = true;
        } //if
    } //SecondNameComboBox_SelectionChanged
} //Query3Window

