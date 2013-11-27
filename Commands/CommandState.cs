using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace L6.Infrastructure.Commands
{
    public enum CommandState
    {
        Created,
        Validated,
        ValidatedByHandlers,
        SendToBL,
        EventsSentForPersitance,
        Completed
    }
}
