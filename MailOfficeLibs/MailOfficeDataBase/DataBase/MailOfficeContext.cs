using MailOfficeEntities.Entities;
using MailOfficeEntities.Entities.Accounts;
using MailOfficeEntities.Entities.Receipts;
using MailOfficeEntities.Entities.Residence;
using Microsoft.EntityFrameworkCore;

namespace MailOfficeDataBase.DataBase;

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
    public DbSet<Receipt> Receipts => Set<Receipt>(); 

    public MailOfficeContext(bool recreateDatabase = false) {
        if (recreateDatabase) 
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