using MailOfficeEntities.Entities.Accounts;
using MailOfficeEntities.Entities.Configurations.Receipts;
using Microsoft.EntityFrameworkCore;

namespace MailOfficeEntities.Entities.Receipts;

// Класс для создания квитанций
[EntityTypeConfiguration(typeof(ReceiptConfiguration))]
public class Receipt {

    public int Id { get; set; }

    // Цена всей покупки
    public decimal Price { get; set; }

    // Дата выдачи квитанции
    public DateTime Issuance { get; set; }

    // Связные свойства для таблицы Person
    public int PersonId { get; set; }
    public virtual Person Person { get; set; }

    // Связное свойство для таблицы ReceiptDetail, связь 1:M
    // Receipt (одна квитанция) -> ReceiptDetail (несколько купленных изданий)
    public virtual List<ReceiptDetail> ReceiptDetails { get; set; } = new(); 

} //Receipt
