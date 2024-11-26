using MailOfficeEntities.Category;

namespace MailOfficeDataBase.Reports;

public record CurrentAccount(
    string Login,
    byte[] Password,
    StaffRole StaffRole
    );
