﻿using MailOfficeEntities.Category;
using MailOfficeEntities.Entities.Configurations.Accounts;
using Microsoft.EntityFrameworkCore;

namespace MailOfficeEntities.Entities.Accounts;

[EntityTypeConfiguration(typeof(PersonConfiguration))]
public class Person {

    public int Id { get; set; } 

    // Имя 
    public string FirstName { get; set; }

    // Фамилия   
    public string SecondName { get; set; }

    // Отчество 
    public string Patronymic { get; set; }

    // Категория (по умолчанию - Guest)
    public PersonCategory Role { get; set; } 

    // Внешняя связь с User  1:1
    public int UserId { get; set; }
    public virtual User User { get; set; }

    // Внешняя связь с Staff 1:1
    public virtual Staff? Staff { get; set; }

    //// Внешняя связь с Subscribers 1:1
    public virtual Subscriber? Subscriber { get; set; } = new();  

    public string FullName => $"{SecondName} {FirstName[0]}.{Patronymic[0]}.";

} //Person