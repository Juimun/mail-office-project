using System.Text.RegularExpressions;

namespace MailOfficeTool.Infrastructure;

public partial class Utils {

    // Создание шаблона - только английские символы
    // Создается один раз при компиляции
    //  ^ - Начало строки - проверяет совпадение с начала строки
    //  + - Квантификатор - проверяет, что один или более символов будут [a-zA-Z]
    //  $ - Якорь - проверяет совпадение до конца строки
    // !!! Все вместе гарантирует, что вся строка будет английская !!!
    [GeneratedRegex("^[a-zA-Z][a-zA-Z0-9]+$")]
    private static partial Regex LoginRegex();

    [GeneratedRegex("^[a-zA-Z0-9!@#$%^&*()_+{}\\[\\]:;<>,.?~\\-|]+$")]
    private static partial Regex PasswordRegex();

    // Проверка строки на правильность логина
    public static bool IsValidLogin(string str) =>
        LoginRegex().IsMatch(str);

    // Проверка строки на наличие только английских символов
    public static bool IsValidPassword(string str) =>
        PasswordRegex().IsMatch(str);

    // Проверка на длину строки с заданным диапазоном
    public static bool IsValidStringLength(string str, int minLength, int maxLength) => 
        str.Length >= minLength && str.Length < maxLength;

} //Utils
