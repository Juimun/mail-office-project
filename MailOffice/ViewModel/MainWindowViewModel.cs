﻿using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Windows;
using MailOffice.Infrastructure;
using MailOffice.View;
using MailOffice.View.Queries;
using MailOfficeControllers.Controllers;
using MailOfficeDataBase.DataBase;
using MailOfficeEntities.Entities;
using MailOfficeTool.Entities;
using MailOfficeTool.Infrastructure;

namespace MailOffice.ViewModel;

public class MainWindowViewModel : INotifyPropertyChanged {

    // Контроллеры для таблиц БД
    private DatabaseDisplayController _dataController;
    private DatabaseQueries _dataQueries; 

    public MainWindow HostWindow { get; set; }

    #region Аккаунт
    private bool _isLoggedIn;
    public bool IsLoggedIn {
        get => _isLoggedIn;
        set => SetField(ref _isLoggedIn, value);
    }

    private UserJson? _currentAccount; 
    public UserJson? CurrentAccount
    {
        get => _currentAccount;
        set => SetField(ref _currentAccount, value);
    }
    #endregion

    public MainWindowViewModel(
        MainWindow hostWindow, DatabaseDisplayController dataController, DatabaseQueries dataQueries)   
    { 
        (HostWindow, _dataController, _dataQueries) = 
            (hostWindow, dataController, dataQueries);

        LoadPublicationPage(1);

        UpdateDataGridSources();
    } // MainWindowViewModel

    #region Команды

    public RelayCommand ExitCommand => new(
        obj => Application.Current.Shutdown(),
        obj => true
    );

    public RelayCommand SignAccountCommand => new(
        obj => SignAccount(),
        obj => true
    );

    public RelayCommand CloseAccountCommand => new(
        obj => CloseAccount(),
        obj => true
    );

    public RelayCommand ChangeAccountCommand => new(
        obj => ChangeAccount(),
        obj => true
    ); 

    public RelayCommand ShowProfileCommand => new( 
        obj => ShowProfile(),
        obj => true
    );

    public RelayCommand ShowSubscribersCommand => new( 
        obj => ShowSubscribers(),
        obj => true
    ); 

    public RelayCommand GenerateTextEntitiesCommand => new(
        obj => GenerateTextEntities(),
        obj => true
    );

    public RelayCommand LeftTabCommand => new(
        obj => LeftTab(),
        obj => true
    );

    public RelayCommand RightTabCommand => new(
        obj => RightTab(),
        obj => true
    );

    public RelayCommand Query1Command => new(
        obj => ShowQuery1(),
        obj => true
    );

    public RelayCommand Query2Command => new( 
        obj => ShowQuery2(),
        obj => true
    );

    public RelayCommand Query3Command => new(
        obj => ShowQuery3(),
        obj => true
    );

    public RelayCommand Query4Command => new(
        obj => ShowQuery4(),
        obj => true
    );

    public RelayCommand Query5Command => new(
        obj => ShowQuery5(),
        obj => true
    );

    public RelayCommand Query6Command => new(
        obj => ShowQuery6(),
        obj => true
    ); 

    public RelayCommand ReportCommand => new(
        obj => ShowReport(), 
        obj => true
    );

    public RelayCommand BackPageCommand => new( 
        obj => BackPage(),
        obj => true
    );

    public RelayCommand NextPageCommand => new(
        obj => NextPage(),
        obj => true
    );

    public RelayCommand FirstPageCommand => new(
        obj => FirstPage(),
        obj => true
    );

    public RelayCommand LastPageCommand => new( 
        obj => LastPage(),
        obj => true
    );
    #endregion

    #region Запросы
    // Вывод запроса № 1 
    private void ShowQuery1()
    {
        HostWindow.TbcMain.SelectedIndex = 1;
        HostWindow.TblQueries.Text = _dataController.ShowQuery1();
    } //ShowQuery1

    // Вывод запроса № 2 
    private void ShowQuery2()
    {
        var query2 = new Query2Window();
        query2.ShowDialog();

        if (query2.DialogResult == true)
        {
            HostWindow.TbcMain.SelectedIndex = 1;
            HostWindow.TblQueries.Text = _dataController.ShowQuery2(
                query2.StreetComboBox.Text,
                query2.HouseNumberComboBox.Text
            );
        } //if
    } //ShowQuery2

    // Вывод запроса № 3 
    //Какие газеты выписывает гражданин с указанной фамилией, именем, отчеством?
    private void ShowQuery3()
    {
        var query3 = new Query3Window();
        query3.ShowDialog();

        if (query3.DialogResult == true)
        {
            // Делаем вкладку активной и выводим результат
            HostWindow.TbcMain.SelectedIndex = 1;
            HostWindow.TblQueries.Text = _dataController.ShowQuery3(
                query3.SecondNameComboBox.Text,
                query3.NameComboBox.Text,
                query3.PatronymicComboBox.Text
            );
        } //if

    } //ShowQuery3

    // Вывод запроса № 4 
    private void ShowQuery4()
    {
        HostWindow.TbcMain.SelectedIndex = 1;
        HostWindow.TblQueries.Text = _dataController.ShowQuery4();
    } //ShowQuery4

    // Вывод запроса № 5 
    private void ShowQuery5()
    {
        HostWindow.TbcMain.SelectedIndex = 1;
        HostWindow.TblQueries.Text = _dataController.ShowQuery5();
    } //ShowQuery5

    // Вывод запроса № 6 
    private void ShowQuery6()
    {
        HostWindow.TbcMain.SelectedIndex = 1;
        HostWindow.TblQueries.Text = _dataController.ShowQuery6();
    } //ShowQuery6
    #endregion

    #region Постраничный вывод
    // Размер страницы
    private int _pageSize = 100;

    // Всего записей в таблице
    private int _totalRecords;

    // Список публикаций
    private List<Publication> _entities;
    public List<Publication> Entities
    {
        get => _entities;
        set => SetField(ref _entities, value);
    }

    // Текущая страница
    private int _currentPage;
    public int CurrentPage
    {
        get => _currentPage;
        set => SetField(ref _currentPage, value);
    }

    // Всего страниц
    private int _totalPages;
    public int TotalPages
    {
        get => _totalPages;
        set => SetField(ref _totalPages, value);
    }

    private void FirstPage() {
        CurrentPage = 1;
        LoadPublicationPage(CurrentPage);
        UpdateDataGridSources();
    } //FirstPage

    private void LastPage() {
        CurrentPage = TotalPages;
        LoadPublicationPage(CurrentPage);
        UpdateDataGridSources();
    } //LastPage

    private void BackPage() {
        CurrentPage = CurrentPage > 1 ? CurrentPage - 1 : TotalPages;
        LoadPublicationPage(CurrentPage);
        UpdateDataGridSources();
    } //BackPage

    private void NextPage() {
        CurrentPage = CurrentPage < TotalPages ? CurrentPage + 1 : 1;
        LoadPublicationPage(CurrentPage);
        UpdateDataGridSources();
    } //NextPage

    // Создание одной страницы из таблицы Publication
    private void LoadPublicationPage(int pageNumber) {

        // Выбранная страница
        CurrentPage = pageNumber;

        // Вычисление сдвига
        int offset = (pageNumber - 1) * _pageSize;

        // Список сужностей на "странице"
        Entities = new List<Publication>(_dataQueries.GetSelectPagePublication(offset, _pageSize));

        // Вычисляем общее количество страниц
        _totalRecords = _dataQueries.GetTotalPublicationRecords();
        TotalPages = (int)Math.Ceiling((double)_totalRecords / _pageSize);
    } //LoadPublicationPage
    #endregion

    private void LeftTab() {
        if (HostWindow.TbcMain.SelectedIndex > 0)
            HostWindow.TbcMain.SelectedIndex--;
        else
            HostWindow.TbcMain.SelectedIndex = HostWindow.TbcMain.Items.Count - 1;
    } //LeftTab

    private void RightTab() {
        if (HostWindow.TbcMain.SelectedIndex < HostWindow.TbcMain.Items.Count - 1)
            HostWindow.TbcMain.SelectedIndex++;
        else
            HostWindow.TbcMain.SelectedIndex = 0;
    } //RightTab

    // Разделение и привязка в DataGrid
    private void UpdateDataGridSources() {
        int halfCount = Entities.Count / 2; 

        HostWindow.SelectedFirstPartDataGrid.ItemsSource = Entities
            .Take(halfCount)
            .ToList();

        HostWindow.SelectedSecondPartDataGrid.ItemsSource = Entities
            .Skip(halfCount)
            .ToList();
    } //UpdateDataGridSources

    // Вход в аккаунт
    public void SignAccount() {
        var auth = new AuthorizationWindow();
        auth.ShowDialog();

        if (auth.DialogResult == true && 
            _dataQueries.IsAuthenticate(auth.LoginTextBox.Text, Utils.GetBytes(auth.PasswordTextBox.Text))) 
        {
           
            // Сохранение данных
            IsLoggedIn = true;
            CurrentAccount = new UserJson(Login: auth.LoginTextBox.Text,
                Password: Utils.GetBytes(auth.PasswordTextBox.Text));

            // Отображение профиля
            ShowProfile();
        }
    } //SignAccount

    // Смена аккаунта
    public void ChangeAccount() {
        var change = new ChangeAccountWindow();
        change.ShowDialog();
    } //ChangeAccount

    // Выход из аккаунта
    public void CloseAccount() {
        IsLoggedIn = false;
        CurrentAccount = null;

        HostWindow.TblProfile.Text = HostWindow.TblReports.Text = string.Empty;
    } //CloseAccount

    // Генерация случайных тестовых значений
    private void GenerateTextEntities() {
        // Генерация данных в MailOfficeDataSeeder (Консольное приложение в проекте)
        Process.Start(App.DataSeederPath).WaitForExit();

        LoadPublicationPage(1);

        // Вывод выбранной страницы
        UpdateDataGridSources();
    } //GenerateTextEntities

    // Отобразить профиль
    private void ShowProfile() {
        if (IsLoggedIn) {
            HostWindow.TbcMain.SelectedIndex = 3;
            HostWindow.TblProfile.Text = _dataController.ShowCurrentProfile(CurrentAccount!.Login, CurrentAccount.Password);
        } //if
    } //ShowProfile

    // Отобразить список подписок
    private void ShowSubscribers() {
        if (IsLoggedIn) {
            HostWindow.TbcMain.SelectedIndex = 3;
            HostWindow.TblProfile.Text = _dataController.ShowSubscriptionsCurrentUser(CurrentAccount!.Login, CurrentAccount.Password);
        } //if
    } //ShowProfile

    // Отобразить отчет
    private void ShowReport() {

        // Доступ к отчетам ТОЛЬКО у Director и Administrator
        if (IsLoggedIn && _dataQueries.IsDirector(CurrentAccount!.Login, CurrentAccount.Password)){
            HostWindow.TbcMain.SelectedIndex = 2;
            HostWindow.TblReports.Text = _dataController.ShowReport(CurrentAccount!.Login, CurrentAccount.Password);
        } //if
    } //ShowReport

    #region INotifyPropertyChanged
    public event PropertyChangedEventHandler? PropertyChanged;

    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null) =>
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

    protected bool SetField<T>(ref T field, T value, [CallerMemberName] string? propertyName = null)
    {
        if (EqualityComparer<T>.Default.Equals(field, value)) return false;
        field = value;
        OnPropertyChanged(propertyName);
        return true;
    }
    #endregion

} //MainWindowViewModel
