using MailOfficeEntities.Category;
using MailOfficeEntities.Entities.Accounts;
using MailOfficeTool.Infrastructure;


namespace MailOfficeFactory.Factories;

public static partial class Factory {

    // Создание случайной сущности Person 
    public static Person GetPerson(int userId) {
        var gender = Utils.GetRandom(0, 1);
        return new() {
            FirstName = GetRandomFirstName(gender),
            SecondName = GetRandomSurname(gender),
            Patronymic = GetRandomPatronymic(gender),
            Role = GetRandomRole(),
            PreviousRole = PersonCategory.Registered,
            UserId = userId,
        };
    } 

    // Создание списка сущностей Person 
    public static List<Person> GetPeople(List<User> users, Func<int, Person> getPerson) => users
            .Select(p => getPerson(p.Id))
            .ToList();

    // Генератор случайной роли PersonCategory
    private static int _countStaff = 0, _countSubscriber = 0;
    private static PersonCategory GetRandomRole() {
        while (true) {
            switch (Utils.GetRandom(1, 3)) {
                case 1:
                    if (_countStaff < 50) {
                        _countStaff++;
                        return PersonCategory.Staff;
                    }
                    continue;
                case 2:
                    if (_countSubscriber < 100) {
                        _countSubscriber++;
                        return PersonCategory.Subscriber;
                    }
                    continue;
                default:
                    return PersonCategory.Registered;
            } //switch
        } //while
    }

    // Выбор "случайного" отчества из тестового массива
    private static string GetRandomPatronymic(int gender) => gender == 1 ?
            Patronymics[Utils.GetRandom(0, Patronymics.Length - 1)] + "ич" : Patronymics[Utils.GetRandom(0, Patronymics.Length - 1)] + "на";

    // Выбор "случайной" фамилии из тестового массива
    private static string GetRandomSurname(int gender) => gender == 1 ?
        Surnames[Utils.GetRandom(0, Surnames.Length - 1)] : Surnames[Utils.GetRandom(0, Surnames.Length - 1)] + "а";

    // Выбор "случайного" имени из тестового массива
    private static string GetRandomFirstName(int gender) => gender == 1 ?
        MaleNames[Utils.GetRandom(0, MaleNames.Length - 1)] : FemaleNames[Utils.GetRandom(0, FemaleNames.Length - 1)];

    #region ФИО для тестов 
    private static readonly string[] MaleNames = [
        "Александр", "Алексей", "Анатолий", "Андрей", "Антон", "Аркадий", "Артем", "Артур", "Борис", "Вадим",
        "Валентин", "Валерий", "Владлен", "Владимир", "Владислав", "Вячеслав", "Георгий", "Геннадий", "Герман",
        "Григорий", "Даниил", "Денис", "Дмитрий", "Евгений", "Егор", "Захар", "Иван", "Игорь", "Илья", "Кирилл",
        "Константин", "Леонид", "Максим", "Михаил", "Никита", "Николай", "Олег", "Павел", "Петр", "Роман",
        "Руслан", "Сергей", "Семен", "Станислав", "Степан", "Тимофей", "Федор", "Юрий", "Яков", "Эдуард"
    ];

    private static readonly string[] FemaleNames = [
        "Агата", "Александра", "Алина", "Алла", "Анастасия", "Ангелина", "Анна", "Антонина", "Валентина", "Валерия",
        "Варвара", "Вера", "Виктория", "Виолетта", "Галина", "Дарья", "Евгения", "Елена", "Елизавета", "Жанна",
        "Зинаида", "Ирина", "Карина", "Катерина", "Клавдия", "Ксения", "Лариса", "Лидия", "Любовь", "Людмила",
        "Маргарита", "Мария", "Марина", "Марта", "Наталья", "Нина", "Оксана", "Ольга", "Полина", "София",
        "Светлана", "Татьяна", "Юлия", "Яна", "Жанна", "Людмила", "Алёна", "Ирина", "Анжела", "Диана"
    ];

    private static readonly string[] Surnames = [
        "Иванов", "Смирнов", "Кузнецов", "Васильев", "Петров", "Михайлов", "Федоров", "Соколов", "Попов", "Семёнов",
        "Новиков", "Морозов", "Волков", "Богданов", "Козлов", "Зайцев", "Захаров", "Александров", "Николаев", "Афанасьев",
        "Павлов", "Никитин", "Орлов", "Макаров", "Голубев", "Соловьёв", "Егоров", "Кузьмин", "Дмитриев", "Ильин",
        "Абрамов", "Сергеев",  "Григорьев",  "Степанов",  "Сидоров",  "Киселёв", "Алексеев", "Андреев", "Романов", "Беляев",
        "Королёв",  "Ефимов",  "Костин",  "Заварзин",  "Фёдоров",  "Кудрявцев",  "Леонов",  "Крюков",  "Шумилов",  "Лобанов"
    ];

    private static readonly string[] Patronymics = [
        "Александров", "Алексеев", "Анатольев", "Андреев", "Антонов", "Аркадьев", "Артемов", "Артуров",
        "Борисов", "Вадимов", "Валентинов", "Валерьев", "Владимиров", "Владиславов", "Вячеславов",
        "Георгиев", "Геннадьев", "Германов", "Григорьев", "Данилов", "Денисов", "Дмитриев",
        "Евгеньев", "Егоров", "Захаров", "Иванов", "Игорьев", "Ильин", "Кириллов", "Константинов",
        "Леонидов", "Максимов", "Михайлов", "Никитин", "Николаев", "Олегов", "Павлов", "Петров",
        "Романов", "Русланов", "Сергеев", "Семенов", "Станиславов", "Степанов", "Тимофеев",
        "Федоров", "Юрьев", "Яковлев", "Эдуардов"
    ];
    #endregion
} //FactoryPerson