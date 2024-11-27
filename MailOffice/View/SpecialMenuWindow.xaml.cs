﻿using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using MailOffice.ViewModel;
using MailOfficeDataBase.DataBase;

namespace MailOffice.View;

/// <summary>
/// Логика взаимодействия для SpecialMenuWindow.xaml
/// </summary>
public partial class SpecialMenuWindow : Window {

    private SolidColorBrush _originalBackgroundBrush;
    private SolidColorBrush _originalForegroundBrush;

    public SpecialMenuWindow() {
        InitializeComponent();

        DataContext = new SpecialMenuWindowViewModel(this, new DatabaseQueries());
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
}

