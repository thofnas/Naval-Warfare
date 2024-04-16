using EventBus;
using Events;
using Map;
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
        private VisualElement _storePanelsContainer = new();
        private Label _moneyLabel;

        private readonly Wallet _wallet;

        private EventBinding<OnStoreItemUnlocked> _onStoreItemUnlocked;

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
            
            _onStoreItemUnlocked = new EventBinding<OnStoreItemUnlocked>(OnStoreItemUnlocked);
            EventBus<OnStoreItemUnlocked>.Register(_onStoreItemUnlocked);
        }

        public override void OnExit()
        {
            base.OnExit();
            
            _storePanelsContainer.Clear();
            OnThemeChangedBinding.Remove(UpdateStoreContent);
            
            EventBus<OnStoreItemUnlocked>.Deregister(_onStoreItemUnlocked);
        }

        protected sealed override void GenerateView() 
        {
            base.GenerateView();
            
            _container = Root.CreateChild("container");
            _storePanelsContainer = _container.CreateChild("store-panels-container");
            VisualElement buttonsContainer = _container.CreateChild("buttons-container");

            VisualElement moneyContainer = _container.CreateChild("money-container");
            VisualElement moneyIcon = moneyContainer.CreateChild("money-icon");
            _moneyLabel = moneyContainer.CreateChild<Label>("money-text");

            _moneyLabel.text = _wallet.GetCurrentMoney().ToString();
            
            StyledButton backToMainMenuButton = new(SelectedTheme.PlayerTheme, buttonsContainer,
                () => StateMachine.SwitchState(MainMenuUIManager.MainMenuState), "back-button")
            {
                text = "Back"
            };
        }

        private void UpdateStoreContent()
        {
            _storePanelsContainer.Clear();

            StorePanel islandsStorePanel = MainMenuUIManager.StorePanelFactory.Create(_storeContent.IslandsThemeItems, MapType.Islands);
            _storePanelsContainer.Add(islandsStorePanel);
            StorePanel oceanStorePanel = MainMenuUIManager.StorePanelFactory.Create(_storeContent.OceanThemeItems, MapType.Ocean);
            _storePanelsContainer.Add(oceanStorePanel);
        }
        
        private void OnStoreItemUnlocked(OnStoreItemUnlocked e)
        {
            if (e.IsPurchasable) return;
            
            UpdateStoreContent();
        }
    }
}