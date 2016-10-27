using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

public class InboxCommand : IDatabaseObject
{
    [CompilerGenerated]
    private string <_id>k__BackingField;
    public static readonly List<InboxCommandIdType> BENEVOLENT_INBOXCOMMANDS;
    public InboxCommandIdType CommandId;
    public InboxCommandParameterType Parameters;
    public string playerId;

    static InboxCommand()
    {
        List<InboxCommandIdType> list = new List<InboxCommandIdType>();
        list.Add(InboxCommandIdType.GiftShopProduct);
        list.Add(InboxCommandIdType.RewardResource);
        list.Add(InboxCommandIdType.RewardShopProduct);
        list.Add(InboxCommandIdType.RewardTournamentCardPack);
        BENEVOLENT_INBOXCOMMANDS = list;
    }

    public string _id
    {
        [CompilerGenerated]
        get
        {
            return this.<_id>k__BackingField;
        }
        [CompilerGenerated]
        set
        {
            this.<_id>k__BackingField = value;
        }
    }
}

