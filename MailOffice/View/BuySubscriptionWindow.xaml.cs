using MailOffice.ViewModel;
using MailOfficeControllers.Controllers;
using MailOfficeDataBase.DataBase;
using MailOfficeEntities.Category;
using MailOfficeEntities.Entities;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;


namespace MailOffice.View;

/// <summary>
/// Логика взаимодействия для BuySubscriptionWindow.xaml
/// </summary>
public partial class BuySubscriptionWindow : Window {

    private SolidColorBrush _originalBackgroundBrush;
    private SolidColorBrush _originalForegroundBrush;

    private DatabaseDisplayController _dataController;  

    public BuySubscriptionWindow(MainWindowViewModel mainWindowViewModel) {
        InitializeComponent();

        _dataController = new DatabaseDisplayController();
        DataContext = new BuySubscriptionWindowViewModel(this, new DatabaseQueries(), mainWindowViewModel);

        // Заполняем комбо бокс вариантами Срока подписки
        foreach (var period in Enum.GetNames(typeof(SubscriptionPeriod))) 
            SubscriptionPeriodComboBox.Items.Add(period);
    }

    // TODO: Пофиксить случайное пересоздание
    private void PublicationDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e) {
        SelectedPublicationsTextBlock.Text = string.Empty;

        // Список выделенных сущностей 
        var selectedPublication = PublicationDataGrid
            .SelectedItems
            .OfType<Publication>()
            .ToList();

        if (selectedPublication.Count > 0) 
            SelectedPublicationsTextBlock.Text = _dataController.ShowPublications(selectedPublication);
    } //PublicationDataGrid_SelectionChanged

    private void UIElement_OnMouseEnter(object sender, MouseEventArgs mouseEventArgs) {
        Button button = (Button)sender;
        _originalBackgroundBrush = (SolidColorBrush)button.Background;
        _originalForegroundBrush = (SolidColorBrush)button.Foreground;

        button.Foreground = Brushes.Black;
    }

    private void UIElement_OnMouseLeave(object sender, MouseEventArgs mouseEventArgs) {
        Button button = (Button)sender;

        // Восстанавливаем исходные цвета
        button.Background = _originalBackgroundBrush;
        button.Foreground = _originalForegroundBrush;
    }

} //BuySubscriptionWindow
