namespace Service
{
    using System;
    using System.Collections.Generic;

    [InboxCommand(InboxCommandIdType.LoadPromotions)]
    public class HandleReloadPromotions : IInboxCommandHandler
    {
        public HandleReloadPromotions(Dictionary<string, object> parameters)
        {
        }

        public override void Execute()
        {
            Binder.TaskManager.StartTask(Binder.PromotionService.LoadPromotions(), null);
        }
    }
}

