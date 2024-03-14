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

        public Store(MainMenuUIManager mainMenuUIManager, StateMachine.StateMachine stateMachine, StyleSheet styleSheet, StoreContent storeContent) : base(mainMenuUIManager, stateMachine)
        {
            Root = CreateDocument(nameof(Store), styleSheet);
            _storeContent = storeContent;
            
            GenerateView();
        }

        protected sealed override VisualElement Root { get; }
        
        public sealed override void GenerateView() 
        {            
            VisualElement container = Root.CreateChild("container");
            VisualElement storeItemsContainer = Root.CreateChild("store-items-container");
            VisualElement buttonsContainer = container.CreateChild("buttons-container");

            foreach (IslandsThemeItem islandsThemeItem in _storeContent.IslandsThemeItems)
            {
                StoreItemView storeItemView = storeItemsContainer.CreateChild<StoreItemView>().Initialize(islandsThemeItem);
                storeItemView.Clicked += view => SelectedThemeSettings.PlayerThemeSettings = view.StoreItem.ThemeSettings;
            }
            
            StyledButton backToMainMenuButton = new(SelectedThemeSettings.PlayerThemeSettings, buttonsContainer,
                () => StateMachine.SwitchState(MainMenuUIManager.MainMenuState), "back-button")
            {
                text = "Back"
            };
        }
    }
}