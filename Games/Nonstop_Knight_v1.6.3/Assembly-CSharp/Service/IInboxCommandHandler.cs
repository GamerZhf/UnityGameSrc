namespace Service
{
    using System;

    public abstract class IInboxCommandHandler
    {
        protected IInboxCommandHandler()
        {
        }

        public abstract void Execute();
    }
}

