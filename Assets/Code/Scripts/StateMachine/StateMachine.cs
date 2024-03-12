using System;
using Zenject;

namespace StateMachine
{
    public class StateMachine : ITickable, IDisposable
    {
        private StateMachine()
        { }

        public IState CurrentState { get; private set; }

        public void Tick()
        {
            CurrentState?.Update();
        }

        public void SwitchState(IState to)
        {
            CurrentState?.OnExit();
            CurrentState = to;
            CurrentState.OnEnter();
        }

        public void SetState(IState state) => SwitchState(state);

        public bool IsCurrentState(IState state) => CurrentState == state;
        
        public void Dispose()
        {
            CurrentState?.OnExit();
            CurrentState?.OnDispose();
        }
    }
}