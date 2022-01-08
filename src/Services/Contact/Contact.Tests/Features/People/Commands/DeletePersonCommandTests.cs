using Contact.API.Entities;
using Contact.API.Features.People.Commands;
using Contact.API.Features.People.Exceptions;
using Contact.API.Infrastructure.Data;
using FluentAssertions;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Contact.Tests.Features.People.Commands;

public class DeletePersonCommandTests
{
    [Fact]
    public async Task DeletePersonCommand_Should_Delete_Person()
    {
        var context = new ContactContext(new DbContextOptionsBuilder<ContactContext>()
            .UseInMemoryDatabase(databaseName: $"{Guid.NewGuid()}")
            .Options);

        var mulder = new Person()
        {
            Name = "Fox",
            Lastname = "Mulder",
            CompanyName = "FBI"
        };

        var scully = new Person()
        {
            Name = "Dana Katherine",
            Lastname = "Scully",
            CompanyName = "FBI"
        };

        await context.AddRangeAsync(mulder, scully);
        await context.SaveChangesAsync();

        var deletePersonCommand = new DeletePersonCommand
        {
            PersonId = mulder.Id
        };

        var deletePersonCommandHandler = new DeletePersonCommandHandler(context, new Mock<IBus>().Object);
        await deletePersonCommandHandler.Handle(deletePersonCommand, CancellationToken.None);

        context.People.Count().Should().Be(1);
    }

    [Fact]
    public async Task DeletePersonCommand_Should_Throw_Not_Found_Exception_When_Person_Not_Found()
    {
        var context = new ContactContext(new DbContextOptionsBuilder<ContactContext>()
            .UseInMemoryDatabase(databaseName: $"{Guid.NewGuid()}")
            .Options);

        var deletePersonCommand = new DeletePersonCommand
        {
            PersonId = Guid.NewGuid()
        };

        var deletePersonCommandHandler = new DeletePersonCommandHandler(context, new Mock<IBus>().Object);

        var action = async () => await deletePersonCommandHandler.Handle(deletePersonCommand, CancellationToken.None);

        await action.Should().ThrowAsync<PersonNotFoundException>();
    }
}
