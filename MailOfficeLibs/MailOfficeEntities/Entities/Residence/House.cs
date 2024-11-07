using MailOfficeEntities.Entities.Accounts;
using MailOfficeEntities.Entities.Configurations.Residence;
using Microsoft.EntityFrameworkCore;

namespace MailOfficeEntities.Entities.Residence;

// Класс для дома в участке
[EntityTypeConfiguration(typeof(HouseConfiguration))]
public class House {

    public int Id { get; set; }

    // Улица/проспект 
    public string Street { get; set; }

    // Номер дома
    public string HouseNumber { get; set; }

    // Внешняя связь с Section
    public int SectionId { get; set; }
    public virtual Section Section { get; set; }

    // Внешняя связь с Subscriber
    public virtual Subscriber Subscriber { get; set; }


    public string Address => $"{Street}, дом {HouseNumber}";

} //House