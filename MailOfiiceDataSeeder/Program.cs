using MailOfficeDataBase.DataBase;

Console.WriteLine($"[{DateTime.Now:HH:MM:ss}] Начало генерации!");

// Перегенерирование тестовых данных 
var data = new DatabaseQueries(new MailOfficeContext(true));
data.AddTestEntities();

Console.WriteLine($"[{DateTime.Now:HH:MM:ss}] Я все сгенерировал! ");
Console.ReadKey();