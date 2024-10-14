using MailOffice.Models.Entities.Configurations.Residence;
using Microsoft.EntityFrameworkCore;

namespace MailOffice.Models.Entities.Residence;

// Класс для дома в участке
[EntityTypeConfiguration(typeof(HouseConfiguration))]
public class House {

    public int Id { get; set; }

    // Улица/проспект 
    public string Street { get; set; }

    // Номер дома
    public string HouseNumber { get; set; }

    #region Внешний ключ и связное свойстов для Section
    public int SectionId { get; set; }
    public virtual Section Section { get; set; }
    #endregion

    public string Adress => $"{Street}, дом {HouseNumber}";

} //House