using States.MainMenuUIStates;
using Themes.Store;
using UI.Elements;
using UnityEngine;
using UnityEngine.UIElements;
using Zenject;

namespace UI
{
    [DisallowMultipleComponent]
    public class MainMenuUIManager : MonoBehaviour
    {
        public SelectedThemeSettings SelectedThemeSettings { get; private set; }
        public MainMenu MainMenuState { get; private set; }
        public Store StoreState { get; private set; }
        public Options OptionsState { get; private set; }
        public StorePanel.Factory StorePanelFactory { get; private set; }
        
        [SerializeField] private StyleSheet _mainMenuStyleSheet;
        [SerializeField] private StyleSheet _storeStyleSheet;
        [SerializeField] private StyleSheet _optionsStyleSheet;
        [SerializeField] private StoreContent _storeContent;
        
        
        private VisualElement _root;
        private StateMachine.StateMachine _stateMachine;
        
        [Inject]
        private void Construct(StateMachine.StateMachine stateMachine, SelectedThemeSettings selectedThemeSettings, StorePanel.Factory storePanelFactory)
        {
            _stateMachine = stateMachine;
            SelectedThemeSettings = selectedThemeSettings;
            StorePanelFactory = storePanelFactory;
        }

        private void Awake()
        {
            MainMenuState = new MainMenu(this, _stateMachine, _mainMenuStyleSheet);
            StoreState = new Store(this, _stateMachine, _storeStyleSheet, _storeContent);
            OptionsState = new Options(this, _stateMachine, _optionsStyleSheet);
                
            _stateMachine.SetState(MainMenuState);
        }
    }
}