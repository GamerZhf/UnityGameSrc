namespace Service
{
    using GameLogic;
    using System;
    using System.Collections.Generic;

    [InboxCommand(InboxCommandIdType.ChangeHeroName)]
    public class HandleChangeHeroName : IInboxCommandHandler
    {
        private string newName;

        public HandleChangeHeroName(Dictionary<string, object> parameters)
        {
            if (parameters.ContainsKey("name"))
            {
                this.newName = parameters["name"].ToString();
            }
        }

        public override void Execute()
        {
            CmdRenamePlayer.ExecuteStatic(GameLogic.Binder.GameState.Player, this.newName);
        }
    }
}

