using MailOfficeDataBase.DataBase;

Console.WriteLine($"[{DateTime.Now:HH:MM:ss}] Подготовка тестовых данных… Это может занять некоторое время.");

// Создание "случайных" тестовых данных 
var data = new DatabaseQueries(new MailOfficeContext(true));
data.AddTestEntities();

// Завершение работы приложения после генерации данных
Environment.Exit(0);