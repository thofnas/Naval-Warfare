using StateMachine;

namespace States.GameplayStates
{
    public abstract class BaseState : IState
    {
        protected readonly TurnSystem TurnSystem;

        protected BaseState(TurnSystem turnSystem)
        {
            TurnSystem = turnSystem;
        }

        public virtual void OnCreated()
        {
        }

        public virtual void OnEnter()
        {
        }

        public virtual void OnExit()
        {
        }

        public virtual void Update()
        {
        }

        public virtual void FixedUpdate()
        {
        }
    }
}