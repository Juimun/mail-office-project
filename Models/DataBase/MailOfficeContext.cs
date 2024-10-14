using Microsoft.EntityFrameworkCore;
using MailOffice.Models.Entities;
using MailOffice.Models.Entities.Residence;
using MailOffice.Models.Entities.Accounts;

namespace MailOffice.Models.DataBase;

// Контекст базы данных
public sealed class MailOfficeContext : DbContext {

    public DbSet<User> Users => Set<User>();
    public DbSet<Person> People => Set<Person>();
    public DbSet<Subscriber> Subscribers => Set<Subscriber>();
    public DbSet<Staff> Staff => Set<Staff>();
    public DbSet<Subscription> Subscriptions => Set<Subscription>();
    public DbSet<Publication> Publications => Set<Publication>();
    public DbSet<Section> Sections => Set<Section>();
    public DbSet<House> Houses => Set<House>();

    public MailOfficeContext()
    {
        Database.EnsureDeleted();
        Database.EnsureCreated();
    } //MailOfficeContext

    // настройка контекста 
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);

        // строка подключения
        optionsBuilder
            // подключение lazy loading, сначала установить NuGet-пакет Microsoft.EntityFrameworkCore.Proxies
            .UseLazyLoadingProxies()
            .UseSqlServer(@"Server=(localdb)\mssqllocaldb;Database=MailOfficeDb;Trusted_Connection=True;");
    } // OnConfiguring 

} //MailOfficeContext