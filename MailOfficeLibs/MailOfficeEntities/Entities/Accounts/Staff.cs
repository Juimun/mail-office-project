using MailOfficeEntities.Category;
using MailOfficeEntities.Entities.Configurations.Accounts;
using MailOfficeEntities.Entities.Residence;
using Microsoft.EntityFrameworkCore;

namespace MailOfficeEntities.Entities.Accounts;

[EntityTypeConfiguration(typeof(StaffConfiguration))]
public class Staff {

    public int Id { get; set; }

    public StaffRole Role { get; set; }

    // Связное свойство для таблицы Person, связь 1:1
    public int PersonId { get; set; }
    public virtual Person Person { get; set; }

    // Связное свойство для таблицы Section, связь 1:1
    public int? SectionId { get; set; } 
    public virtual Section? Section { get; set; }

} //Staff

