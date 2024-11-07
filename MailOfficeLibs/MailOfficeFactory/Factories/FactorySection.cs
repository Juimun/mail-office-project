using MailOfficeEntities.Entities.Residence;

namespace MailOfficeFactory.Factories;

public static partial class Factory {

    // Создание сущности Section
    public static Section GetSection(int sectionId) => new() 
    {
        Id = sectionId,
        Name = GetSectionName(sectionId)
    };

    // Создание списка сущностей Section
    public static List<Section> GetSections(int count, Func<int, Section> getSection) => Enumerable    
        .Range(1, count)
        .Select(getSection)
        .ToList();

    // Создание "случайного" названия участка
    private static string GetSectionName(int nameId) =>
        $"Участок №{nameId}";

} //FactorySection