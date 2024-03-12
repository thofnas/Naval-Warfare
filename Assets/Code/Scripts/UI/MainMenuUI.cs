using StateMachine;
using States.MainMenuUIStates;
using Themes;
using UnityEngine;
using UnityEngine.UIElements;
using Zenject;

namespace UI
{
    [DisallowMultipleComponent]
    public class MainMenuUI : MonoBehaviour
    {
        public ThemeSettings ThemeSettings { get; private set; }
        
        [SerializeField] private StyleSheet _mainMenuStyleSheet;
        private VisualElement _root;
        private StateMachine.StateMachine _stateMachine;
        private MainMenu _mainMenu;
        
        [Inject]
        private void Construct(StateMachine.StateMachine stateMachine, ThemeSettings themeSettings)
        {
            _stateMachine = stateMachine;
            ThemeSettings = themeSettings;
        }

        private void Awake()
        {
            _mainMenu = new MainMenu(this, _mainMenuStyleSheet, _stateMachine);
            
            _stateMachine.SetState(_mainMenu);
        }
    }
}