namespace PYDFramework.StateMachine
{
    public abstract class StateBase
    {
        protected StateMachine StateMachine;

        public StateBase(StateMachine stateMachine)
        {
            StateMachine = stateMachine;
        }

        public virtual void Enter() { }
        public virtual void LogicUpdate(float detalTime) { }
        public virtual void PhysicUpdate(float fixedDeltaTime) { }
        public virtual void Exit() { }
    }

    public abstract class State<_Agent> : StateBase
    {
        protected _Agent agent;
        protected State(_Agent agent,StateMachine stateMachine) : base(stateMachine)
        {
            this.agent = agent;
        }
    }
}
