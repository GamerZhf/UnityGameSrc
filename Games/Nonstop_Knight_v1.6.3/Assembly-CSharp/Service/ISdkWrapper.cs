namespace Service
{
    using System;

    public interface ISdkWrapper
    {
        void Event(ESdkEvent eventName, object param);
        void Purchase(PremiumProduct product);
        void SessionEnd();
        void SessionStart(string trUserId);
    }
}

