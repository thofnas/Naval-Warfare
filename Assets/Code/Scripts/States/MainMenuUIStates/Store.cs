using EventBus;
using Themes.Store;
using UI;
using UI.Elements;
using UnityEngine;
using UnityEngine.UIElements;
using Utilities.Extensions;

namespace States.MainMenuUIStates
{
    public class Store : BaseState
    {
        private readonly StoreContent _storeContent;
        private StoreItem _selectedStoreItem;
        private VisualElement _container;
        private VisualElement _storeItemsContainer = new();

        public Store(MainMenuUIManager mainMenuUIManager, StateMachine.StateMachine stateMachine, StyleSheet styleSheet, StoreContent storeContent) : base(mainMenuUIManager, stateMachine)
        {
            Root = CreateDocument(nameof(Store), styleSheet);
            _storeContent = storeContent;
            
            GenerateView();
        }

        protected sealed override VisualElement Root { get; }

        public override void OnEnter()
        {
            base.OnEnter();

            UpdateStoreContent();
            OnThemeChangedBinding.Add(UpdateStoreContent);
        }

        public override void OnExit()
        {
            base.OnExit();
            
            _storeItemsContainer.Clear();
            OnThemeChangedBinding.Remove(UpdateStoreContent);
        }

        protected sealed override void GenerateView() 
        {
            base.GenerateView();
            
            _container = Root.CreateChild("container");
            _storeItemsContainer = _container.CreateChild("store-items-container");
            VisualElement buttonsContainer = _container.CreateChild("buttons-container");
            
            StyledButton backToMainMenuButton = new(SelectedThemeSettings.PlayerThemeSettings, buttonsContainer,
                () => StateMachine.SwitchState(MainMenuUIManager.MainMenuState), "back-button")
            {
                text = "Back"
            };
        }

        private void UpdateStoreContent()
        {
            _storeItemsContainer.Clear();

            _storeItemsContainer.Add(MainMenuUIManager.StorePanelFactory.Create(_storeContent.IslandsThemeItems));
        }
    }
}