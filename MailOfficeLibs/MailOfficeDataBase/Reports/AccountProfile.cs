using MailOfficeEntities.Category;

namespace MailOfficeDataBase.Reports;

public record AccountProfile(
    int UserId,  
    string Login,
    string FirstName,
    string SecondName,
    string Patronymic,
    PersonCategory PersonRole, 
    StaffRole? StaffRole, 
    string? Street, 
    string? HouseNumber,
    int? SectionId,
    string? SectionName
    );
