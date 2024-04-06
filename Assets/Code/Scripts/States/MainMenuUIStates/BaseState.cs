using EventBus;
using Events;
using Infrastructure;
using StateMachine;
using Themes;
using UI;
using UnityEngine;
using UnityEngine.UIElements;

namespace States.MainMenuUIStates
{
    public abstract class BaseState : IState
    {
        protected abstract VisualElement Root { get; }
        protected MainMenuUIManager MainMenuUIManager { get; }
        protected StateMachine.StateMachine StateMachine { get; }
        protected SelectedSettings SelectedSettings { get; }
        
        protected BaseState(MainMenuUIManager mainMenuUIManager, StateMachine.StateMachine stateMachine)
        {
            StateMachine = stateMachine;
            MainMenuUIManager = mainMenuUIManager;
            SelectedSettings = mainMenuUIManager.SelectedSettings;

            OnThemeChangedBinding = new EventBinding<OnThemeChanged>(GenerateView);
            EventBus<OnThemeChanged>.Register(OnThemeChangedBinding);
        }

        protected readonly EventBinding<OnThemeChanged> OnThemeChangedBinding;

        protected virtual void GenerateView()
        {
            Root.Clear();
        }


        public virtual void OnEnter() => SetVisible(true);

        public virtual void Update() { }

        public virtual void OnExit() => SetVisible(false);
        
        public virtual void OnDispose() 
        {   
            EventBus<OnThemeChanged>.Deregister(OnThemeChangedBinding);
        }

        protected void SetVisible(bool value) => Root.visible = value;
        
        protected static VisualElement CreateDocument(string name, StyleSheet styleSheet)
        {
            var uiDocument = new GameObject(name).AddComponent<UIDocument>();
            uiDocument.panelSettings = GameResources.Instance.UIDocumentPrefab.panelSettings;
            uiDocument.visualTreeAsset = GameResources.Instance.UIDocumentPrefab.visualTreeAsset;
            VisualElement root = uiDocument.rootVisualElement;

            root.styleSheets.Add(styleSheet);

            root.visible = false;
            
            return root;
        }
    }
}