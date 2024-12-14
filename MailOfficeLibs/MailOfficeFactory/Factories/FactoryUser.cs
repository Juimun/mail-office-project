using MailOfficeEntities.Entities.Accounts;
using MailOfficeTool.Infrastructure;


namespace MailOfficeFactory.Factories;

// Фабрика для генерации тестовых данных
public static partial class Factory {

    // Создание сущности User
    public static User GetUser(int userId) => new() {
        Login = GetRandomLogin(userId), 
        Password = Utils.GetBytes(GetRandomPassword(MinLength, MaxLength))
    };

    // Создание списка сущностей Users
    public static List<User> GetUsers(int count, Func<int, User> getUser) => Enumerable   
        .Range(1, count)
        .Select(getUser) 
        .ToList();

    // Генератор типизированного логина
    // Минимальная длина 6, максимальная - 30
    private static string GetRandomLogin(int userId) => $"Login{userId}";

    // Генератор случайного пароля
    // Минимальная длина 8
    private const int MinLength = 8, MaxLength = 15;
    private const string Chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
    private static string GetRandomPassword(int minlength, int maxlength) => new(
        Enumerable.Repeat(Chars, Utils.GetRandom(minlength, maxlength))
            .Select(s => s[Utils.GetRandom(0, Chars.Length - 1)])
            .ToArray());

} //FactoryUser