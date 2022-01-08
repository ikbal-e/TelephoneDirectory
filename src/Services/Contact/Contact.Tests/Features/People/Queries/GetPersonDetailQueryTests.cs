using Contact.API.Entities;
using Contact.API.Features.People.Commands;
using Contact.API.Infrastructure.Data;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Contact.Tests.Features.People.Queries;

public class GetPersonDetailQueryTests
{
    [Fact]
    public async Task GetPeopleQuery_Should__Not_Return_Empty_List_When_People_Exist()
    {
        var context = new ContactContext(new DbContextOptionsBuilder<ContactContext>()
            .UseInMemoryDatabase(databaseName: $"{Guid.NewGuid()}")
            .Options);

        var person1 = new Person { Name = "Name1", Lastname = "Lastname1", CompanyName = "Companyname1" };

        context.People.AddRange(new Person[]
        {
            person1,
            new Person{Name = "Name2", Lastname = "Lastname2", CompanyName = "Companyname2"},
        });

        await context.SaveChangesAsync();

        context.Contacts.AddRange(new ContactInformation[]
        {
            new ContactInformation
            {
                ContactInformationType = API.ValueObjects.ContactInformationType.Location,
                Value = "Location1",
                PersonId = person1.Id
            },
            new ContactInformation
            {
                ContactInformationType = API.ValueObjects.ContactInformationType.PhoneNumber,
                Value = "0001",
                PersonId = person1.Id
            }
        });

        await context.SaveChangesAsync();

        var getPersonDetailQuerydHandler = new GetPersonDetailQuerydHandler(context);
        var getPersonDetailQuery = new GetPersonDetailQuery { PersonId = person1.Id};

        var person = await getPersonDetailQuerydHandler.Handle(getPersonDetailQuery, CancellationToken.None);

        person.Should().NotBeNull();
        person.Name.Should().Be("Name1");
        person.Lastname.Should().Be("Lastname1");
        person.CompanyName.Should().Be("Companyname1");
        person.ContactInformations.Should().HaveCount(2);
    }
}
