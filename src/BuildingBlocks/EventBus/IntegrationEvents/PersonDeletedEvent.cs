﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventBus.IntegrationEvents;

public class PersonDeletedEvent
{
    public Guid PersonIdOnContactService { get; set; }
}
