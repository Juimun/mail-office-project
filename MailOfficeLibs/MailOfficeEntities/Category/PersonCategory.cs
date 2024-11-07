namespace MailOfficeEntities.Category;

// Категория сущностей для Person
// Все типы пользователей
public enum PersonCategory {

    // Гость - минимальное количество прав
    Guest,

    // Зарегестрированный пользователь 
    Registered,

    // Подписчик
    Subscriber,

    // Сотрудник
    Staff,

} //PersonCategory