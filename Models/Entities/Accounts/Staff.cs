using MailOffice.Models.Category;
using MailOffice.Models.Entities.Configurations.Accounts;
using MailOffice.Models.Entities.Residence;
using Microsoft.EntityFrameworkCore;

namespace MailOffice.Models.Entities.Accounts;

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

