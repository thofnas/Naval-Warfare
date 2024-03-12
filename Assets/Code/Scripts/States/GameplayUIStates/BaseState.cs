using StateMachine;
using UI;
using UnityEngine.UIElements;

namespace States.GameplayUIStates
{
    public abstract class BaseState : IState
    {
        protected readonly GameplayUI GameplayUI;

        protected BaseState(GameplayUI gameplayUI)
        {
            GameplayUI = gameplayUI;
        }

        protected abstract VisualElement Root { get; }
        
        public virtual void OnEnter() => SetVisible(true);

        public virtual void OnExit() => SetVisible(false);

        public virtual void Update()
        {
        }

        public abstract void GenerateView();

        protected abstract void SetVisible(bool value);
        
        public virtual void OnDispose() { }
    }
}