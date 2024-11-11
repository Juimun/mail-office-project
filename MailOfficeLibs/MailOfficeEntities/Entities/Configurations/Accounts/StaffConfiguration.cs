using MailOfficeEntities.Entities.Accounts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MailOfficeEntities.Entities.Configurations.Accounts;

public class StaffConfiguration : IEntityTypeConfiguration<Staff> 
{

    public void Configure(EntityTypeBuilder<Staff> builder)
    {

        builder.ToTable("Staff");
        builder.Property(s => s.Id)
            .ValueGeneratedOnAdd();

        builder
            .HasOne(s => s.Person)
            .WithOne(p => p.Staff)
            .HasForeignKey<Staff>(s => s.PersonId);

        builder
            .HasOne(s => s.Section)
            .WithOne(p => p.Staff)
            .HasForeignKey<Staff>(s => s.SectionId);

    } //Configure

} //StaffConfiguration