using MailOffice.Models.Category;
using MailOffice.Models.Entities.Accounts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MailOffice.Models.Entities.Configurations.Accounts;

public class PersonConfiguration : IEntityTypeConfiguration<Person> 
{

    public void Configure(EntityTypeBuilder<Person> builder)
    {

        builder.ToTable("People");
        builder.Property(a => a.Id)
            .ValueGeneratedNever();

        builder
            .Property(p => p.FirstName)
            .HasMaxLength(50)
            .IsRequired();

        builder
            .Property(p => p.SecondName)
            .HasMaxLength(60)
            .IsRequired();

        builder
            .Property(p => p.Patronymic)
            .HasMaxLength(70)
            .IsRequired(false);

        builder.Property(p => p.Role)
            .HasDefaultValue(PersonCategory.Guest);

        builder
            .HasOne(p => p.User)
            .WithOne(u => u.Person)
            .HasForeignKey<Person>(u => u.UserId);

        builder.HasMany(p => p.Subscribers)
            .WithOne(s => s.Person)
            .HasForeignKey(s => s.PersonId);

        List<Person> persons = [
            new() { Id = 1, FirstName = "Александр", SecondName = "Александров", Patronymic = "Александрович", Role = PersonCategory.Staff, UserId = 1 },
            new() { Id = 2, FirstName = "Евгений", SecondName = "Кушнарев", Patronymic = "Олегович", Role = PersonCategory.Staff, UserId = 2 },
            new() { Id = 3, FirstName = "Альберт", SecondName = "Шевронцев", Patronymic = "Александрович", Role = PersonCategory.Staff, UserId = 3 },
            new() { Id = 4, FirstName = "Ольга", SecondName = "Смирнова", Patronymic = "Владимировна", Role = PersonCategory.Staff, UserId = 4 },
            new() { Id = 5, FirstName = "Альберт", SecondName = "Шевронцев", Patronymic = "Александрович", Role = PersonCategory.Staff, UserId = 5 },
        
            new() { Id = 6, FirstName = "Александр", SecondName = "Суровцев", Patronymic = "Иванович", Role = PersonCategory.Staff, UserId = 6 },
            new() { Id = 7, FirstName = "Александр", SecondName = "Александров", Patronymic = "Александрович", Role = PersonCategory.Staff, UserId = 7 },
            new() { Id = 8, FirstName = "Евгений", SecondName = "Старостенко", Patronymic = "Александрович", Role = PersonCategory.Staff, UserId = 8 },
            new() { Id = 9, FirstName = "Михаил", SecondName = "Давыдов", Patronymic = "Олегович", Role = PersonCategory.Staff, UserId = 9 },
            new() { Id = 10, FirstName = "Александр", SecondName = "Александров", Patronymic = "Александрович", Role = PersonCategory.Staff, UserId = 10 },
        
            new() { Id = 11, FirstName = "Анна", SecondName = "Кушнарева", Patronymic = "Сергеевна", Role = PersonCategory.Subscriber, UserId = 11 },
            new() { Id = 12, FirstName = "Антон", SecondName = "Криволапко", Patronymic = "Дмитриевич", Role = PersonCategory.Subscriber, UserId = 12 },
            new() { Id = 13, FirstName = "Евгений", SecondName = "Кушнарев", Patronymic = "Олегович", Role = PersonCategory.Subscriber, UserId = 13 },
            new() { Id = 14, FirstName = "Ольга", SecondName = "Петрова", Patronymic = "Алексеевна", Role = PersonCategory.Subscriber, UserId = 14 },
            new() { Id = 15, FirstName = "Мария", SecondName = "Петрова", Patronymic = "Алексеевна", Role = PersonCategory.Subscriber, UserId = 15 },
        
            new() { Id = 16, FirstName = "Алексей", SecondName = "Сидоров", Patronymic = "Петрович", UserId = 16 },
            new() { Id = 17, FirstName = "Дмитрий", SecondName = "Кузнецов", Patronymic = "Сергеевич", UserId = 17 },
            new() { Id = 18, FirstName = "Олег", SecondName = "Кузнецов", Patronymic = "Олегович", Role = PersonCategory.Staff, UserId = 18 },
            new() { Id = 19, FirstName = "Евгений", SecondName = "Парламов", Patronymic = "Сергеевич", Role = PersonCategory.Staff, UserId = 19 },
            new() { Id = 20, FirstName = "Илья", SecondName = "Кабаш", Patronymic = "Владимирович", Role = PersonCategory.Staff, UserId = 20 },

        ]; //Person
        builder.HasData(persons);

    } //Configure

} //PersonConfiguration