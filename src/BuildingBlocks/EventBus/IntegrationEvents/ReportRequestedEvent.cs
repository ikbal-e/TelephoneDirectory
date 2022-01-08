using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventBus.IntegrationEvents;

public class ReportRequestedEvent
{
    public DateTime RequestedAt { get; set; }
}
