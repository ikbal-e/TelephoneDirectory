﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventBus.IntegrationEvents;

public class ContactInfoDeletedEvent
{
    public Guid ContactIdOnContactService { get; set; }
}
