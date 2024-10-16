using MailOffice.Models.Entities.Accounts;
using MailOffice.Models.Entities.Configurations.Residence;
using Microsoft.EntityFrameworkCore;

namespace MailOffice.Models.Entities.Residence;

// Класс для участка
// Несколько домов объединяются в участок, который обслуживается одним почтальоном
[EntityTypeConfiguration(typeof(SectionConfiguration))]
public class Section {

    public int Id { get; set; }

    // Наименование участка
    public string Name { get; set; }

    // Связное свойство для таблицы Staff, связь 1:1
    public virtual Staff Staff { get; set; } 

    // Связное свойство для таблицы House, связь 1:M
    public virtual List<House> Houses { get; set; } = new();

} //Section 