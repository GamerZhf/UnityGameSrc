using GameLogic;
using System;
using System.Collections.Generic;

public class UpdateResponse
{
    public List<InboxCommand> InboxCommands = new List<InboxCommand>();
    public GameLogic.ServerStats ServerStats;
}

