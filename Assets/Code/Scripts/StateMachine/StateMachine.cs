using System;
using Zenject;

namespace StateMachine
{
    public class StateMachine : ITickable, IDisposable
    {
        private StateMachine()
        { }

        public event Action<IState, IState> OnStateChanged;

        private IState _currentState;
        
        public void Tick() => 
            _currentState?.Update();

        public void SwitchState(IState to)
        {
            if (to == null)
                throw new ArgumentNullException(nameof(to), "Cannot switch to a null state.");

            IState oldState = _currentState;

            oldState?.OnExit();

            to.OnEnter();

            _currentState = to;

            OnStateChanged?.Invoke(oldState, _currentState);
        }

        public void SetState(IState state) => 
            SwitchState(state);

        public bool IsCurrentState(Type stateType) =>
            _currentState?.GetType() == stateType;
        
        public void Dispose()
        {
            _currentState?.OnExit();
            _currentState?.Dispose();
        }
    }
}