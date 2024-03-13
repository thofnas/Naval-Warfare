using UI;
using UI.Elements;
using UnityEngine;
using UnityEngine.UIElements;
using Utilities.Extensions;

namespace States.MainMenuUIStates
{
    public class Store : BaseState
    {
        public Store(MainMenuUIManager mainMenuUIManager, StateMachine.StateMachine stateMachine, StyleSheet styleSheet) : base(mainMenuUIManager, stateMachine)
        {
            Root = CreateDocument(nameof(Store), styleSheet);
            
            GenerateView();
        }

        protected sealed override VisualElement Root { get; }
        
        public sealed override void GenerateView() 
        {            
            VisualElement container = Root.CreateChild("container");
            VisualElement buttonsContainer = container.CreateChild("buttons-container");
            
            StyledButton backToMainMenuButton = new(MainMenuUIManager.ThemeSettings, buttonsContainer,
                () => StateMachine.SwitchState(MainMenuUIManager.MainMenuState), "back-button")
            {
                text = "Back"
            };
        }
    }
}