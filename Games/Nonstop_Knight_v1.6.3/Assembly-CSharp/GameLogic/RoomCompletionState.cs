namespace GameLogic
{
    using System;

    public class RoomCompletionState : FiniteStateMachine.State
    {
        public RoomCompletionState() : base(Enum.GetName(typeof(GameplayState), GameplayState.ROOM_COMPLETION), 7)
        {
            base.EnterMethod = new Action(this.onEnter);
            base.UpdateMethod = new Action<float>(this.onUpdate);
            base.ExitMethod = new Action(this.onExit);
        }

        public void onEnter()
        {
        }

        public void onExit()
        {
        }

        public void onUpdate(float dt)
        {
        }
    }
}

