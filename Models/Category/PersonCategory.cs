namespace MailOffice.Models.Category;

// Категория сущностей для Person
// Все типы пользователей
public enum PersonCategory {

    // Гость - минимальное количество прав
    Guest,

    // Зарегестрированный пользователь (TODO: пока не используется!)
    Registered,

    // Подписчик
    Subscriber,

    // Сотрудник
    Staff,

} //PersonCategory