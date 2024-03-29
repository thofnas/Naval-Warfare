﻿using Themes;
using Themes.Store;
using UI;
using UI.Elements;
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

        private Wallet _wallet;
        private Label _moneyLabel;

        public Store(MainMenuUIManager mainMenuUIManager, 
            StateMachine.StateMachine stateMachine, 
            StyleSheet styleSheet, 
            StoreContent storeContent, 
            Wallet wallet) : base(mainMenuUIManager, stateMachine)
        {
            Root = CreateDocument(nameof(Store), styleSheet);
            _storeContent = storeContent;
            _wallet = wallet;

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

            VisualElement moneyContainer = _container.CreateChild("money-container");
            VisualElement moneyIcon = moneyContainer.CreateChild("money-icon");
            _moneyLabel = moneyContainer.CreateChild<Label>("money-text");

            _moneyLabel.text = _wallet.GetCurrentMoney().ToString();
            
            StyledButton backToMainMenuButton = new(SelectedThemeSettings.PlayerTheme, buttonsContainer,
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