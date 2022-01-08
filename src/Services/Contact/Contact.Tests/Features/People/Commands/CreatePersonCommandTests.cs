using Contact.API.Features.People.Commands;
using Contact.API.Infrastructure.Data;
using FluentAssertions;
using FluentValidation;
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
using static Contact.API.Features.People.Commands.CreatePersonCommandHandler;

namespace Contact.Tests.Features.People.Commands;

public class CreatePersonCommandTests
{
    [Fact]
    public async Task CreatePersonCommand_Should_Throw_Validation_Error_When_Empty_Name_is_Given()
    {
        var createPersonCommand = new CreatePersonCommand
        {
            Name = string.Empty,
            Lastname = string.Empty,
            CompanyName = string.Empty,
        };

        var validator = new CreatePersonCommandValidator();
        var action = async () => await validator.ValidateAndThrowAsync(createPersonCommand);

        await action.Should().ThrowAsync<ValidationException>();
    }

    [Fact]
    public async Task CreatePersonCommand_Should_Create_Person()
    {
        var context = new ContactContext(new DbContextOptionsBuilder<ContactContext>()
            .UseInMemoryDatabase(databaseName: $"{Guid.NewGuid()}")
            .Options);

        var createPersonCommand = new CreatePersonCommand
        {
            Name = "Name1",
            Lastname = "Lastname1",
            CompanyName = "CompanyName1"
        };

        var createPersonCommandHandler = new CreatePersonCommandHandler(context, new Mock<IBus>().Object);
        var createdPerson = await createPersonCommandHandler.Handle(createPersonCommand, CancellationToken.None);

        createdPerson.Should().NotBeNull();
        createdPerson.Id.Should().NotBe(Guid.Empty);
        createdPerson.Name.Should().Be("Name1");
        createdPerson.Lastname.Should().Be("Lastname1");
        createdPerson.CompanyName.Should().Be("CompanyName1");
    }
}
