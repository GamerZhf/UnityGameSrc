namespace GameLogic
{
    using System;

    public class GameplayRetirementState : FiniteStateMachine.State
    {
        public GameplayRetirementState() : base(Enum.GetName(typeof(GameplayState), GameplayState.RETIREMENT), 10)
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

