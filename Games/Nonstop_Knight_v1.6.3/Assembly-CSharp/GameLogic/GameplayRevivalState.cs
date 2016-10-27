namespace GameLogic
{
    using System;

    public class GameplayRevivalState : FiniteStateMachine.State
    {
        public GameplayRevivalState() : base(Enum.GetName(typeof(GameplayState), GameplayState.REVIVAL), 9)
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

