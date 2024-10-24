using MailOffice.Models.Category;
using MailOffice.Models.Entities.Accounts;

namespace MailOffice.Infrastructure;

// Фабрика для генерации тестовых данных
public static class Factory {

    // Начальное значение для Id в БД
    private const int SourceId = 1,

        // Минимальная длина пароля
        MinLength = 5,

        // Максимальная длина пароля
        MaxLength = 15;

    // Создание сущности User
    public static User GetUser(int userId) => new()  
        { Id = userId, Login = GetRandomLogin(userId), Password = Utils.GetBytes(GetRandomPassword(MinLength, MaxLength)) };

    // Создание сущности Person 
    public static Person GetPerson(int personId, int userId) => new()  
    { 
        Id = personId, FirstName = GetRandomFirstName(), SecondName = GetRandomSurname(),
        Patronymic = GetRandomPatronymic(), Role = GetRandomRole(), UserId = userId
    };

    // Генератор типизированного логина
    private static string GetRandomLogin(int userId) => $"Login{userId}";

    // Генератор случайного пароля
    private const string Chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
    private static string GetRandomPassword(int minlength, int maxlength) => new(
        Enumerable.Repeat(Chars, Utils.GetRandom(minlength, maxlength))
            .Select(s => s[Random.Shared.Next(s.Length)])
            .ToArray());

    // Генератор случайной роли PersonCategory
    private static PersonCategory GetRandomRole() => 
        Utils.GetRandom(1, 4) switch 
    {
        1 => PersonCategory.Registered,
        2 => PersonCategory.Subscriber,
        3 => PersonCategory.Staff,
        _ => PersonCategory.Guest
    };

    // Выбор "случайного" отчества из тестового массива
    private static string GetRandomPatronymic() =>
        Patronymics[Utils.GetRandom(0, Patronymics.Length)];

    // Выбор "случайной" фамилии из тестового массива
    private static string GetRandomSurname() =>
        Surnames[Utils.GetRandom(0, Surnames.Length)];

    // Выбор "случайного" имени из тестового массива
    private static string GetRandomFirstName() =>
        Names[Utils.GetRandom(0, Names.Length)]; 

    #region ФИО для тестов 
    private static readonly string[] Names = {
        "Алина", "Артем", "Валерия", "Виктор", "Даниил", "Дарья", "Егор", "Екатерина", "Иван", "Ирина",
        "Кирилл", "Кристина", "Максим", "Мария", "Николай", "Наталья", "Олег", "Ольга", "Павел", "Полина",
        "Роман", "Светлана", "Сергей", "София", "Тимофей", "Юлия", "Александр", "Анастасия", "Андрей", "Анна",
        "Борис", "Вера", "Дмитрий", "Евгения", "Георгий", "Елена", "Игорь", "Ксения", "Леонид", "Любовь",
        "Михаил", "Марина", "Петр", "Татьяна", "Руслан", "Елизавета", "Алексей", "Вадим", "Глеб", "Евгения"
    };

    private static readonly string[] Surnames = {
        "Иванов", "Смирнов", "Кузнецов", "Васильев", "Петров", "Михайлов", "Федоров", "Соколов", "Попов", "Семёнов",
        "Новиков", "Морозов", "Волков", "Богданов", "Козлов", "Зайцев", "Захаров", "Александров", "Николаев", "Афанасьев",
        "Павлов", "Никитин", "Орлов", "Макаров", "Голубев", "Соловьёв", "Егоров", "Кузьмин", "Дмитриев", "Ильин",
        "Абрамов", "Сергеев",  "Григорьев",  "Степанов",  "Сидоров",  "Киселёв", "Алексеев", "Андреев", "Романов", "Беляев",
        "Королёв",  "Ефимов",  "Костин",  "Заварзин",  "Фёдоров",  "Кудрявцев",  "Леонов",  "Крюков",  "Шумилов",  "Лобанов"
    };

    private static readonly string[] Patronymics = {
        "Александрович", "Алексеевич", "Андреевич", "Артемович", "Борисович",
        "Вадимович", "Викторович", "Владимирович", "Дмитриевич", "Евгеньевич",
        "Игоревич", "Ильич", "Константинович", "Максимович", "Николаевич",
        "Олегович", "Павлович", "Петрович", "Сергеевич", "Тимофеевич",
        "Александровна", "Алексеевна", "Андреевна", "Артемовна", "Борисовна",
        "Вадимовна", "Викторовна", "Владимировна", "Дмитриевна", "Евгеньевна",
        "Игоревна", "Ильинична", "Константиновна", "Максимовна", "Николаевна",
        "Олеговна", "Павловна", "Петровна", "Сергеевна", "Тимофеевна"
    };
    #endregion
} //Factory