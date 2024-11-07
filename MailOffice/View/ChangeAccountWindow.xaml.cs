using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using MailOffice.ViewModel.Authentication;

namespace MailOffice.View;

/// <summary>
/// Логика взаимодействия для ChangeAccountWindow.xaml
/// </summary>
public partial class ChangeAccountWindow : Window {

    private SolidColorBrush _originalBackgroundBrush;
    private SolidColorBrush _originalForegroundBrush;

    public ChangeAccountWindow() {
        InitializeComponent();

        DataContext = new ChangeAccountViewModel(this);
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

} //ChangeAccountWindow