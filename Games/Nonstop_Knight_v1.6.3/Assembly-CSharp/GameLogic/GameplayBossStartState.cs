namespace GameLogic
{
    using System;

    public class GameplayBossStartState : FiniteStateMachine.State
    {
        public GameplayBossStartState() : base(Enum.GetName(typeof(GameplayState), GameplayState.BOSS_START), 5)
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

