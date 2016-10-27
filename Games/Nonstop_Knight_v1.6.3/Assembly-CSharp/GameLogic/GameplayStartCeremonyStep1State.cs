namespace GameLogic
{
    using System;

    public class GameplayStartCeremonyStep1State : FiniteStateMachine.State
    {
        public GameplayStartCeremonyStep1State() : base(Enum.GetName(typeof(GameplayState), GameplayState.START_CEREMONY_STEP1), 1)
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

