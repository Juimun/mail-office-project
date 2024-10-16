﻿using System.Windows;
using MailOffice.Controllers;
using MailOffice.ViewModel;

namespace MailOffice.View;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window {

    private DatabaseDisplayController _data;  

    public MainWindow() {
        InitializeComponent();

        _data = new DatabaseDisplayController();

        DataContext = new MainWindowViewModel(this, _data);
    } 

} //MainWindow