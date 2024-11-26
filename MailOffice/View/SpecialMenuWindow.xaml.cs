using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using MailOffice.ViewModel;

namespace MailOffice.View;

/// <summary>
/// Логика взаимодействия для SpecialMenuWindow.xaml
/// </summary>
public partial class SpecialMenuWindow : Window {

    private SolidColorBrush _originalBackgroundBrush;
    private SolidColorBrush _originalForegroundBrush;

    public SpecialMenuWindow() {
        InitializeComponent();

        DataContext = new SpecialMenuWindowViewModel(this);
    }

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

