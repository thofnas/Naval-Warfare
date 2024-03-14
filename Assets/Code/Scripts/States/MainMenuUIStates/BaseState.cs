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
        
        protected SelectedThemeSettings SelectedThemeSettings { get; }
        
        protected BaseState(MainMenuUIManager mainMenuUIManager, StateMachine.StateMachine stateMachine)
        {
            StateMachine = stateMachine;
            MainMenuUIManager = mainMenuUIManager;
            SelectedThemeSettings = mainMenuUIManager.SelectedThemeSettings;
        }

        public abstract void GenerateView();


        public virtual void OnEnter() => SetVisible(true);

        public virtual void Update() { }

        public virtual void OnExit() => SetVisible(false);
        
        public virtual void OnDispose() { }

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