namespace MailOfficeTool.Entities;

//Оформление подписки связано с выдачей клиенту квитанции,
// в которой указывается общая стоимость подписки, что выписано, и на какой срок.
public record Receipt(
    decimal AllPrice, 
    List<string> PublicationName,
    DateTime StartDate,
    List<int> Duration
    );
