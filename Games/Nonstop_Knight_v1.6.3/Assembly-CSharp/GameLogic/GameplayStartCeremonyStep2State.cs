namespace GameLogic
{
    using System;

    public class GameplayStartCeremonyStep2State : FiniteStateMachine.State
    {
        public GameplayStartCeremonyStep2State() : base(Enum.GetName(typeof(GameplayState), GameplayState.START_CEREMONY_STEP2), 2)
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

