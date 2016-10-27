namespace GameLogic
{
    using System;

    public interface IRenewableResourceSystem
    {
        float getSecondsUntilRefresh(ResourceType resourceType);
    }
}

