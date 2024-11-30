namespace MailOfficeEntities.Entities.Receipts;

// Для вывода квитанций в DataGrid
public record ReceiptWithDetail(
    decimal Price,
    DateTime Issuance,
    List<ReceiptDetail> ReceiptDetails
    );


