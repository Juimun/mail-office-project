using MailOfficeEntities.Entities.Accounts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MailOfficeEntities.Entities.Configurations.Accounts;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {

        builder.ToTable("Users");
        builder.Property(u => u.Id)
            .ValueGeneratedOnAdd();

        builder
            .Property(u => u.Login)
            .HasMaxLength(16)
            .IsRequired()
            .IsUnicode(false);

        // настройка уникальности для поля Login
        // настроить ограничение поля Login для User: 
        // https://stackru.com/questions/47356295/ogranichenie-unikalnosti-svojstv-bez-dobavleniya-neizmennosti-s-ef7?ysclid=ln0f77vhkv579643823 
        builder
            .HasIndex(u => u.Login)
            .IsUnique();

        builder
            .Property(u => u.Password)
            .HasMaxLength(55)
            .IsRequired()
            .IsUnicode(false);

    } //Configure

} //UserConfiguration