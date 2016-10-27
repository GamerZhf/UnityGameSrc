namespace GameLogic
{
    using System;

    public class GameplayEndedState : FiniteStateMachine.State
    {
        public GameplayEndedState() : base(Enum.GetName(typeof(GameplayState), GameplayState.ENDED), 11)
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

