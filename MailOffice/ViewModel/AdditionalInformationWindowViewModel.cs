using MailOffice.Infrastructure;
using MailOffice.View;
using MailOfficeDataBase.DataBase;
using MailOfficeEntities.Category;
using MailOfficeEntities.Entities;
using System.Collections.ObjectModel;
using System.Windows;

namespace MailOffice.ViewModel;

public class AdditionalInformationWindowViewModel {

    private AdditionalInformationWindow HostWindow;
    private DatabaseQueries _databaseQueries;
    private BuySubscriptionWindowViewModel _buySubscriptionWindowViewModel;
    private MainWindowViewModel _mainWindowViewModel;

    public AdditionalInformationWindowViewModel(
       AdditionalInformationWindow hostWindow, DatabaseQueries databaseQueries, BuySubscriptionWindowViewModel buySubscriptionWindowViewModel)
    {
        (HostWindow, _databaseQueries, _buySubscriptionWindowViewModel) =
            (hostWindow, databaseQueries, buySubscriptionWindowViewModel);

        // Заполняем комбо бокс наименованиями участков
        HostWindow.SectionNameComboBox.ItemsSource = _databaseQueries.GetAllSectionNames();
    } //SpecialMenuWindowViewModel

    public RelayCommand ConfirmCommand => new( 
        obj => ConfirmSubscription(),
        obj =>
             !string.IsNullOrWhiteSpace(HostWindow.FirstNameTextBox.Text)   && HostWindow.FirstNameTextBox.Text != "Введите ваше имя" &&
             !string.IsNullOrWhiteSpace(HostWindow.SecondNameTextBox.Text)  && HostWindow.SecondNameTextBox.Text != "Введите вашу фамилию" &&
             !string.IsNullOrWhiteSpace(HostWindow.PatronymicTextBox.Text)  && HostWindow.PatronymicTextBox.Text != "Введите свое отчество" &&
             !string.IsNullOrWhiteSpace(HostWindow.StreetTextBox.Text)      && HostWindow.StreetTextBox.Text != "Введите наименование улицы" &&
             !string.IsNullOrWhiteSpace(HostWindow.HouseNumberTextBox.Text) && HostWindow.HouseNumberTextBox.Text != "Введите номер дома" &&
              HostWindow.SectionNameComboBox.SelectedItem != null
    );

    public RelayCommand CanselCommand => new(
        obj => HostWindow.Close(),
        obj => true
    );


    private void ConfirmSubscription() {

        // Создаем квитанцию
        _databaseQueries.GetNewReceipt(
             _buySubscriptionWindowViewModel._mainWindowViewModel.CurrentAccount!.Login,
             _buySubscriptionWindowViewModel._mainWindowViewModel.CurrentAccount.Password,

             _buySubscriptionWindowViewModel.SelectedPublications,
             _buySubscriptionWindowViewModel.SelectedSubscriptionPeriod!.Value,

             HostWindow.FirstNameTextBox.Text,
             HostWindow.SecondNameTextBox.Text,
             HostWindow.PatronymicTextBox.Text,
             HostWindow.SectionNameComboBox.SelectedItem.ToString()!,
             HostWindow.StreetTextBox.Text,
             HostWindow.HouseNumberTextBox.Text
         );

        // Закрываем все
        HostWindow.Close();
        _buySubscriptionWindowViewModel.HostWindow.Close();

    } //ConfirmSubscription

} //AdditionalInformationWindowViewModel
