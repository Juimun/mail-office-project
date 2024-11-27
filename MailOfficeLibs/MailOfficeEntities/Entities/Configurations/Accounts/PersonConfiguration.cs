﻿using MailOfficeEntities.Category;
using MailOfficeEntities.Entities.Accounts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MailOfficeEntities.Entities.Configurations.Accounts;

public class PersonConfiguration : IEntityTypeConfiguration<Person> 
{

    public void Configure(EntityTypeBuilder<Person> builder)
    {

        builder.ToTable("People");
        builder.Property(a => a.Id)
            .ValueGeneratedOnAdd();
        
        builder
            .Property(p => p.FirstName)
            .HasMaxLength(50)
            .IsRequired(false);

        builder
            .Property(p => p.SecondName)
            .HasMaxLength(60)
            .IsRequired(false);

        builder
            .Property(p => p.Patronymic)
            .HasMaxLength(70)
            .IsRequired(false);

        builder.Property(p => p.Role)
            .HasDefaultValue(PersonCategory.Guest);

        builder.Property(p => p.PreviousRole)
           .HasDefaultValue(PersonCategory.Guest);

        builder
            .HasOne(p => p.User)
            .WithOne(u => u.Person)
            .HasForeignKey<Person>(u => u.UserId);

    } //Configure

} //PersonConfiguration