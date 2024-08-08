using Data;
using EventBus;
using Events;
using Infrastructure;
using StateMachine;
using UnityEngine.UIElements;

namespace UI.MainMenuUIStates
{
    public abstract class BaseState : IState
    {
        protected abstract VisualElement Root { get; }
        protected MainMenuUIManager MainMenuUIManager { get; }
        protected TextData TextData => MainMenuUIManager.LanguageData.TextData;
        protected StateMachine.StateMachine StateMachine { get; }
        protected SelectedTheme SelectedTheme { get; }

        protected readonly EventBinding<OnThemeChanged> OnThemeChangedBinding;
        private readonly EventBinding<OnLanguageLoaded> _onLanguageLoaded;

        protected BaseState(MainMenuUIManager mainMenuUIManager, StateMachine.StateMachine stateMachine)
        {
            StateMachine = stateMachine;
            MainMenuUIManager = mainMenuUIManager;
            SelectedTheme = mainMenuUIManager.SelectedTheme;
            
            OnThemeChangedBinding = new EventBinding<OnThemeChanged>(ClearAndGenerateUI);
            EventBus<OnThemeChanged>.Register(OnThemeChangedBinding);

            _onLanguageLoaded = new EventBinding<OnLanguageLoaded>(ClearAndGenerateUI);
            EventBus<OnLanguageLoaded>.Register(_onLanguageLoaded);
        }

        protected abstract void GenerateUI();
        
        public virtual void OnEnter() => SetVisible(true);

        public virtual void Update() { }

        public virtual void OnExit() => SetVisible(false);
        
        public virtual void Dispose() 
        {   
            EventBus<OnThemeChanged>.Deregister(OnThemeChangedBinding);
            EventBus<OnLanguageLoaded>.Deregister(_onLanguageLoaded);
        }

        protected void SetVisible(bool value) => Root.visible = value;

        protected void ClearAndGenerateUI()
        {
            Root.Clear();
            
            GenerateUI();
        }
    }
}