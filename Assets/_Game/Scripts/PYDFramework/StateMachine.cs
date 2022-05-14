namespace PYDFramework.StateMachine
{
    public class StateMachine
    {
        public StateBase currentState { get; private set; }

        public void Init(StateBase stateDefault)
        {
            currentState = stateDefault;
            currentState?.Enter();
        }

        public void ChangeState(StateBase newState)
        {
            currentState?.Exit();
            currentState = newState;
            currentState?.Enter();
        }
    }
}
