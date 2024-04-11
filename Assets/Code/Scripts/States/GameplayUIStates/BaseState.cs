using StateMachine;
using UI;
using UnityEngine;
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
        
        public virtual void OnEnter() => SetVisible(true);

        public virtual void OnExit() => SetVisible(false);

        public virtual void Update()
        {
        }

        public abstract void GenerateView();

        
        public virtual void Dispose() { }        
        
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