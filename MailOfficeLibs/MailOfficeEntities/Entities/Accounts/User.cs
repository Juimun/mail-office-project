using MailOfficeEntities.Entities.Configurations.Accounts;
using Microsoft.EntityFrameworkCore;

namespace MailOfficeEntities.Entities.Accounts;

//Класс для входа в приложение
[EntityTypeConfiguration(typeof(UserConfiguration))]
public class User {

    public int Id { get; set; }

    public required string Login { get; set; }

    public required byte[] Password { get; set; }

    public string? Avatar { get; set; }

    // Внешняя связь с Person  1:1 
    public virtual Person Person { get; set; }

} //User
