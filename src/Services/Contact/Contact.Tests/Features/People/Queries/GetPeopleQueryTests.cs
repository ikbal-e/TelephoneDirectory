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

public class GetPeopleQueryTests
{
    [Fact]
    public async Task GetPeopleQuery_Should__Not_Return_Empty_List_When_People_Exist()
    {
        var context = new ContactContext(new DbContextOptionsBuilder<ContactContext>()
            .UseInMemoryDatabase(databaseName: $"{Guid.NewGuid()}")
            .Options);

        context.People.AddRange(new Person[]
        {
            new Person{Name = "Name", Lastname = "Lastname", CompanyName = "Companyname"}
        });

        await context.SaveChangesAsync();

        var getPeopleQuerydHandler = new GetPeopleQuerydHandler(context);
        var getPeopleQuery = new GetPeopleQuery();

        var people = await getPeopleQuerydHandler.Handle(getPeopleQuery, CancellationToken.None);

        people.Should().NotBeNullOrEmpty();
        people.First().Id.Should().NotBe(Guid.Empty);
        people.First().Name.Should().NotBeNullOrEmpty();
        people.First().Lastname.Should().NotBeNullOrEmpty();
        people.First().CompanyName.Should().NotBeNullOrEmpty();
    }
}
