using MailOffice.Models.Category;
using MailOffice.Models.Entities.Accounts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MailOffice.Models.Entities.Configurations.Accounts;

public class StaffConfiguration : IEntityTypeConfiguration<Staff> 
{

    public void Configure(EntityTypeBuilder<Staff> builder)
    {

        builder.ToTable("Staff");
        builder.Property(s => s.Id)
            .ValueGeneratedNever();

        builder
            .HasOne(s => s.Person)
            .WithOne(p => p.Staff)
            .HasForeignKey<Staff>(s => s.PersonId);

        builder
            .HasOne(s => s.Section)
            .WithOne(p => p.Staff)
            .HasForeignKey<Staff>(s => s.SectionId);

        List<Staff> staff = [
            new() { Id = 1, PersonId = 1, Role = StaffRole.Administrator},
            new() { Id = 2, PersonId = 2, Role = StaffRole.Director},
            new() { Id = 3, PersonId = 3, Role = StaffRole.Operator},
            new() { Id = 4, PersonId = 18, SectionId = 1, Role = StaffRole.Postman},
            new() { Id = 5, PersonId = 19, SectionId = 2, Role = StaffRole.Postman},
            new() { Id = 6, PersonId = 20, SectionId = 3, Role = StaffRole.Postman},
            new() { Id = 7, PersonId = 4, SectionId = 4, Role = StaffRole.Postman},
            new() { Id = 8, PersonId = 5, SectionId = 5, Role = StaffRole.Postman},
        ];
        builder.HasData(staff);
    } //Configure

} //StaffConfiguration