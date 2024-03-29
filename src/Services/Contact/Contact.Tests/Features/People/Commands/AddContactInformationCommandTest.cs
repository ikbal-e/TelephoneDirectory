﻿using Contact.API.Entities;
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

public class AddContactInformationCommandTest
{
    [Fact]
    public async Task AddContactInformationCommand_Should_Add_New_Contact_Info()
    {
        var context = new ContactContext(new DbContextOptionsBuilder<ContactContext>()
           .UseInMemoryDatabase(databaseName: $"{Guid.NewGuid()}")
           .Options);

        var michael = new Person()
        {
            Name = "Michael",
            Lastname = "Scott",
            CompanyName = "Dunder Mifflin Paper Company, Inc"
        };

        await context.AddAsync(michael);
        await context.SaveChangesAsync();

        var addContactInformationCommand = new AddContactInformationCommand
        {
            PersonId = michael.Id,
            ContactInformationType = API.ValueObjects.ContactInformationType.Location,
            Value = "Scranton"
        };

        var addContactInformationCommandHandler = new AddContactInformationCommandHandler(context, new Mock<IBus>().Object);
        var contactInformation = await addContactInformationCommandHandler.Handle(addContactInformationCommand, CancellationToken.None);

        contactInformation.Should().NotBeNull();
        contactInformation.ContactInformationId.Should().NotBe(Guid.Empty);
        contactInformation.PersonId.Should().Be(michael.Id);
        contactInformation.Value.Should().Be("Scranton");
    }

    [Fact]
    public async Task AddContactInformationCommand_Should_Throw_NotFoud_Error_When_Person_Not_Exists()
    {
        var context = new ContactContext(new DbContextOptionsBuilder<ContactContext>()
            .UseInMemoryDatabase(databaseName: $"{Guid.NewGuid()}")
            .Options);

        var addContactInformationCommand = new AddContactInformationCommand
        {
            PersonId = Guid.NewGuid()
        };

        var addContactInformationCommandHandler = new AddContactInformationCommandHandler(context, new Mock<IBus>().Object);

        var action = async () => await addContactInformationCommandHandler.Handle(addContactInformationCommand, CancellationToken.None);

        await action.Should().ThrowAsync<PersonNotFoundException>();
    }
}
