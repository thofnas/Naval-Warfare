using StateMachine;
using UI;
using UnityEngine.UIElements;

namespace States.GameplayUIStates
{
    public abstract class BaseState : IState
    {
        protected readonly GameplayUIManager GameplayUIManager;

        protected BaseState(GameplayUIManager gameplayUIManager)
        {
            GameplayUIManager = gameplayUIManager;
        }

        protected abstract VisualElement Root { get; }

        public virtual void OnCreated()
        {
            GenerateView();
            SetVisible(false);
        }

        public virtual void OnEnter() => SetVisible(true);

        public virtual void OnExit() => SetVisible(false);

        public virtual void Update()
        {
        }

        public virtual void FixedUpdate()
        {
        }

        protected abstract void GenerateView();

        protected abstract void SetVisible(bool value);
    }
}