﻿using MailOfficeDataBase.DataBase;
using MailOfficeTool.Infrastructure;
using System.IO;
using System.Windows;

namespace MailOffice;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application {

    public static string SourceFilePath = AppDomain.CurrentDomain.BaseDirectory;
    public static string SavesFolderPath = Path.Combine(SourceFilePath, "Saves");
    public static string AccountsJsonPath = Path.Combine(SavesFolderPath, "accounts.json");
    public static string AccountsTxtPath = Path.Combine(SavesFolderPath, "accounts.txt"); 
    public static string DocumentsFolderPath = Path.Combine(SavesFolderPath, "Documents");
    public static string ReportsFolderPath = Path.Combine(DocumentsFolderPath, "Reports");
    public static string StatementsFolderPath = Path.Combine(DocumentsFolderPath, "Statements");
    public static string DataSeederPath = GetDataSeederPath();

    protected override void OnStartup(StartupEventArgs e) {
        base.OnStartup(e);

        // Создание папки для сохранений
        if (!Directory.Exists(SavesFolderPath)) 
            Directory.CreateDirectory(SavesFolderPath);

        // Создание подпапки для документов
        if (!Directory.Exists(DocumentsFolderPath))
            Directory.CreateDirectory(DocumentsFolderPath);

        // Создание подпапки для отчетов  
        if (!Directory.Exists(ReportsFolderPath))
            Directory.CreateDirectory(ReportsFolderPath);

        // Создание подпапки для справок
        if (!Directory.Exists(StatementsFolderPath))
            Directory.CreateDirectory(StatementsFolderPath);

        // !!! Исключительно для тестов приложения !!!
        Utils.SaveAsTxt(new DatabaseQueries().GetAllAccountAuthorization(), AccountsTxtPath,
            "   Логин аккаунта   |   Пароль аккаунта   |   Роль персонала\n");

    } //OnStartup

    // Создание пути к файлу MailOfiiceDataSeeder.exe
    private static string GetDataSeederPath() {
        var dataSeederPath = SourceFilePath;
        for (var cnt = 0; cnt < 5; cnt++)
            dataSeederPath = Path.GetDirectoryName(dataSeederPath);

        return Path.Combine(dataSeederPath!, @"MailOfiiceDataSeeder\bin\Debug\net8.0\MailOfiiceDataSeeder.exe");
    } //DataSeederPath
} //App