using System;
using System.Collections.Generic;

public class MultiInboxCommand
{
    public InboxCommandIdType CommandId;
    public InboxCommandParameterType Parameters;
    public List<string> playerIds = new List<string>();
}

