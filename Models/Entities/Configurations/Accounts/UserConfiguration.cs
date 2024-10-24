using MailOffice.Infrastructure;
using MailOffice.Models.Entities.Accounts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MailOffice.Models.Entities.Configurations.Accounts;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {

        builder.ToTable("Users");
        builder.Property(u => u.Id)
            .ValueGeneratedNever();

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

        // Начальное заполнение таблицы БД
        List<User> users = [
            new() { Id = 1,  Login = "Login1",  Password = Utils.GetBytes("agsdage")},
            new() { Id = 2,  Login = "Login2",  Password = Utils.GetBytes("hadfhadf")},
            new() { Id = 3,  Login = "Login3",  Password = Utils.GetBytes("nsfgnf")},
            new() { Id = 4,  Login = "Login4",  Password = Utils.GetBytes("nsgfn")},
            new() { Id = 5,  Login = "Login5",  Password = Utils.GetBytes("cvnxcv")},

            new() { Id = 6,  Login = "Login6",  Password = Utils.GetBytes("kty")},
            new() { Id = 7,  Login = "Login7",  Password = Utils.GetBytes("dkhgkdnh")},
            new() { Id = 8,  Login = "Login8",  Password = Utils.GetBytes("mdghmhgm")},
            new() { Id = 9,  Login = "Login9",  Password = Utils.GetBytes("bujufdjhn")},
            new() { Id = 10, Login = "Login10", Password = Utils.GetBytes("sfgnfsj")},

            new() { Id = 11, Login = "Login11", Password = Utils.GetBytes("vsdVS")},
            new() { Id = 12, Login = "Login12", Password = Utils.GetBytes("fnggf")},
            new() { Id = 13, Login = "Login13", Password = Utils.GetBytes("mvbmvb")},
            new() { Id = 14, Login = "Login14", Password = Utils.GetBytes("lhjphj")},
            new() { Id = 15, Login = "Login15", Password = Utils.GetBytes("gvbdsg")},

            new() { Id = 16, Login = "Login16", Password = Utils.GetBytes(",jh,hj,")},
            new() { Id = 17, Login = "Login17", Password = Utils.GetBytes("mnfghdmc")},
            new() { Id = 18, Login = "Login18", Password = Utils.GetBytes("gjiodfsghihsz")},
            new() { Id = 19, Login = "Login19", Password = Utils.GetBytes("38-4mb6qb")},
            new() { Id = 20, Login = "Login20", Password = Utils.GetBytes("9o0g450,obz")}
        ]; //users
        builder.HasData(users);
    } //Configure

} //UserConfiguration