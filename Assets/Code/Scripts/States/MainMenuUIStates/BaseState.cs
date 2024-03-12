using StateMachine;
using UI;
using UnityEngine.UIElements;

namespace States.MainMenuUIStates
{
    public abstract class BaseState : IState
    {
        protected readonly MainMenuUI MainMenuUIInstance;
        public StateMachine.StateMachine StateMachine { get; }
        protected abstract VisualElement Root { get; }
        
        protected BaseState(MainMenuUI mainMenuUI, StateMachine.StateMachine stateMachine)
        {
            MainMenuUIInstance = mainMenuUI;
            StateMachine = stateMachine;
        }

        public abstract void GenerateView();
        protected abstract void SetVisible(bool value);

        public virtual void OnEnter() => SetVisible(true);

        public virtual void Update() { }

        public virtual void OnExit() => SetVisible(false);
        
        public virtual void OnDispose() { }
    }
}