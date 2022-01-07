using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventBus.IntegrationEvents;

public class PersonCreatedEvent
{
    public Guid PersonIdOnContactService { get; set; }
    public string Name { get; set; }
    public string Lastname { get; set; }
    public string Company { get; set; }
}
