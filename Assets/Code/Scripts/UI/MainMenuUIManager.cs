using System;
using EventBus;
using Events;
using StateMachine;
using States.MainMenuUIStates;
using Themes;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UIElements;
using Zenject;

namespace UI
{
    [DisallowMultipleComponent]
    public class MainMenuUIManager : MonoBehaviour
    {
        public ThemeSettings ThemeSettings { get; private set; }
        
        [SerializeField] private StyleSheet _mainMenuStyleSheet;
        [SerializeField] private StyleSheet _storeStyleSheet;
        [SerializeField] private StyleSheet _optionsStyleSheet;
        private VisualElement _root;
        private StateMachine.StateMachine _stateMachine;
        public MainMenu MainMenuState { get; private set; }
        public Store StoreState { get; private set; }
        public Options OptionsState { get; private set; }
        
        [Inject]
        private void Construct(StateMachine.StateMachine stateMachine, ThemeSettings themeSettings)
        {
            _stateMachine = stateMachine;
            ThemeSettings = themeSettings;
        }

        private void Awake()
        {
            MainMenuState = new MainMenu(this, _stateMachine, _mainMenuStyleSheet);
            StoreState = new Store(this, _stateMachine, _storeStyleSheet);
            OptionsState = new Options(this, _stateMachine, _optionsStyleSheet);
                
            _stateMachine.SetState(MainMenuState);
        }
    }
}