using MailOfficeEntities.Category;
using MailOfficeEntities.Entities.Accounts;
using MailOfficeTool.Infrastructure;

namespace MailOfficeFactory.Factories;

public static partial class Factory {

    // Создание сущности Staff
    public static Staff GetStaff(int personId, int sectionId) { 
        var role = GetRandomStaffRole();
        return new Staff() 
        {
            PersonId = personId,
            Role = role,
            SectionId = role == StaffRole.Postman ? sectionId : null
        };
    } //GetStaff

    // Создание списка сущностей Staff 
    public static List<Staff> GetStaff(List<Person> people, Func< int, int, Staff> getStaff) => people
            .Where(p => p.Role == PersonCategory.Staff) 
            .Select((p, index) => getStaff(p.Id, index + 1))  
            .ToList();

    // Генератор случайной роли StaffRole
    private static int _countAdmin = 0, _countDirector = 0, _countOperator = 0;
    private static StaffRole GetRandomStaffRole() {
        while (true) {
            switch (Utils.GetRandom(1, 4)) {
                case 1:
                    if (_countAdmin < 1) {
                        _countAdmin++;
                        return StaffRole.Administrator;
                    }
                    continue;
                case 2:
                    if (_countDirector < 1) {
                        _countDirector++;
                        return StaffRole.Director;
                    }
                    continue;
                case 3:
                    if (_countOperator < 15) {
                        _countOperator++;
                        return StaffRole.Operator;
                    }
                    continue;
                default:
                    return StaffRole.Postman;
            } //switch
        } //while
    } //GetRandomStaffRole
} //FactoryStaff