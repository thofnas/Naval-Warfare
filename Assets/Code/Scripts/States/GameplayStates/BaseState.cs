using StateMachine;

namespace States.GameplayStates
{
    public abstract class BaseState : IState
    {
        protected BaseState(StateMachine.StateMachine stateMachine)
        {
            StateMachine = stateMachine;
        }
        
        public StateMachine.StateMachine StateMachine { get; }

        public virtual void OnEnter()
        {
        }

        public virtual void OnExit()
        {
        }


        public virtual void Update()
        {
        }
        
        public virtual void Dispose() { }
    }
}