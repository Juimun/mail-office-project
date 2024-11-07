using MailOfficeEntities.Category;
using MailOfficeEntities.Entities.Accounts;
using MailOfficeTool.Infrastructure;


namespace MailOfficeFactory.Factories; 

public static partial class Factory {

    // Создание случайной сущности Person 
    public static Person GetPerson(int personId, int userId) => new()
        {
            Id = personId,
            FirstName = GetRandomFirstName(),
            SecondName = GetRandomSurname(),
            Patronymic = GetRandomPatronymic(),
            Role = GetRandomRole(),
            UserId = userId,
        }; 
   

    // Создание списка сущностей Person 
    public static List<Person> GetPeople(int count, Func<int, int, Person> getPerson) => Enumerable
        .Range(1, count)
        .Select(p => getPerson(p, p))
        .ToList();

    // Генератор случайной роли PersonCategory
    private static int _countStaff = 0, _countSubscriber = 0;
    private static PersonCategory GetRandomRole() {
       while (true) {
           switch (Utils.GetRandom(1, 4)) {
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
               case 3:
                   return PersonCategory.Registered;
                default:
                   return PersonCategory.Guest;
           } //switch
       } //while
    }

// Выбор "случайного" отчества из тестового массива
private static string GetRandomPatronymic() => 
        Patronymics[Utils.GetRandom(0, Patronymics.Length - 1)];

    // Выбор "случайной" фамилии из тестового массива
    private static string GetRandomSurname() =>
        Surnames[Utils.GetRandom(0, Surnames.Length - 1)];

    // Выбор "случайного" имени из тестового массива
    private static string GetRandomFirstName() =>
        Names[Utils.GetRandom(0, Names.Length - 1)];

    #region ФИО для тестов 
    private static readonly string[] Names = [
        "Алина", "Артем", "Валерия", "Виктор", "Даниил", "Дарья", "Егор", "Екатерина", "Иван", "Ирина",
        "Кирилл", "Кристина", "Максим", "Мария", "Николай", "Наталья", "Олег", "Ольга", "Павел", "Полина",
        "Роман", "Светлана", "Сергей", "София", "Тимофей", "Юлия", "Александр", "Анастасия", "Андрей", "Анна",
        "Борис", "Вера", "Дмитрий", "Евгения", "Георгий", "Елена", "Игорь", "Ксения", "Леонид", "Любовь",
        "Михаил", "Марина", "Петр", "Татьяна", "Руслан", "Елизавета", "Алексей", "Вадим", "Глеб", "Евгения"
    ];

    private static readonly string[] Surnames = [
        "Иванов", "Смирнов", "Кузнецов", "Васильев", "Петров", "Михайлов", "Федоров", "Соколов", "Попов", "Семёнов",
        "Новиков", "Морозов", "Волков", "Богданов", "Козлов", "Зайцев", "Захаров", "Александров", "Николаев", "Афанасьев",
        "Павлов", "Никитин", "Орлов", "Макаров", "Голубев", "Соловьёв", "Егоров", "Кузьмин", "Дмитриев", "Ильин",
        "Абрамов", "Сергеев",  "Григорьев",  "Степанов",  "Сидоров",  "Киселёв", "Алексеев", "Андреев", "Романов", "Беляев",
        "Королёв",  "Ефимов",  "Костин",  "Заварзин",  "Фёдоров",  "Кудрявцев",  "Леонов",  "Крюков",  "Шумилов",  "Лобанов"
    ];

    private static readonly string[] Patronymics = [
        "Александрович", "Алексеевич", "Андреевич", "Артемович", "Борисович",
        "Вадимович", "Викторович", "Владимирович", "Дмитриевич", "Евгеньевич",
        "Игоревич", "Ильич", "Константинович", "Максимович", "Николаевич",
        "Олегович", "Павлович", "Петрович", "Сергеевич", "Тимофеевич",
        "Александровна", "Алексеевна", "Андреевна", "Артемовна", "Борисовна",
        "Вадимовна", "Викторовна", "Владимировна", "Дмитриевна", "Евгеньевна",
        "Игоревна", "Ильинична", "Константиновна", "Максимовна", "Николаевна",
        "Олеговна", "Павловна", "Петровна", "Сергеевна", "Тимофеевна"
    ];
    #endregion
} //FactoryPerson