using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventBus.IntegrationEvents;

public class ContactInfoCreatedEvent
{
    public Guid PersonIdOnContactService { get; set; }
    public Guid ContactIdOnContactService { get; set; }
    public int ContactInformationType { get; set; }
    public string Value { get; set; }
}
