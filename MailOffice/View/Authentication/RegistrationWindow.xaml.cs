﻿using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using MailOffice.ViewModel.Authentication;

namespace MailOffice.View;

/// <summary>
/// Логика взаимодействия для RegistrationWindow.xaml
/// </summary>
public partial class RegistrationWindow : Window {

    private SolidColorBrush _originalBackgroundBrush;
    private SolidColorBrush _originalForegroundBrush;

    public RegistrationWindow() {
        InitializeComponent();

        DataContext = new RegistrationViewModel(this);
    }

    private void TextBox_OnGotFocus(object sender, RoutedEventArgs e) {
        var textBox = (TextBox)sender;
        if (textBox.Text is "Введите логин" or "Введите пароль" or "Введите пароль повторно")
            textBox.Text = string.Empty;
    } //TextBox_OnGotFocus

    private void TextBox_OnLostFocus(object sender, RoutedEventArgs e) {
        var textBox = (TextBox)sender;
        if (string.IsNullOrWhiteSpace(textBox.Text))
            textBox.Text = textBox.Name switch {
                "PasswordRepeatTextBox" => "Введите пароль повторно",
                "PasswordTextBox" => "Введите пароль",
                "LoginTextBox" => "Введите логин",
                _ => textBox.Text
            };
    } //TextBox_OnLostFocus

    private void UIElement_OnMouseEnter(object sender, MouseEventArgs e) {
        Button button = (Button)sender;
        _originalBackgroundBrush = (SolidColorBrush)button.Background;
        _originalForegroundBrush = (SolidColorBrush)button.Foreground;

        button.Foreground = Brushes.Black;
    }


    private void UIElement_OnMouseLeave(object sender, MouseEventArgs e) {
        Button button = (Button)sender;

        // Восстанавливаем исходные цвета
        button.Background = _originalBackgroundBrush;
        button.Foreground = _originalForegroundBrush;
    }
} //RegistrationWindow