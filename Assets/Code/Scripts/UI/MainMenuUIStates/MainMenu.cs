using Misc;
using UI.Elements;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
using Utilities.Extensions;

namespace UI.MainMenuUIStates
{
    public class MainMenu : BaseState
    {
        public MainMenu(MainMenuUIManager mainMenuUIManager, StateMachine.StateMachine stateMachine, StyleSheet styleSheet) : base(mainMenuUIManager, stateMachine)
        {
            Root = VisualElementHelper.CreateDocument(nameof(MainMenu), styleSheet);

            GenerateUI();
        }

        protected sealed override VisualElement Root { get; }

        protected sealed override void GenerateUI() 
        {
            VisualElement container = Root.CreateChild("container");
            VisualElement contentContainer = container.CreateChild("content-container");

            VisualElement titleContainer = contentContainer.CreateChild("title-container", "flex-center");
            titleContainer.Add(new Label(TextData.Title));

            VisualElement buttonsContainer = contentContainer.CreateChild("buttons-container");

            StyledButton startGameButton = new(SelectedTheme.PlayerTheme,
                buttonsContainer,
                () => SceneManager.LoadScene("Gameplay"),
                "start-button")
            {
                text = TextData.PlayButton
            };

            StyledButton storeButton = new(SelectedTheme.PlayerTheme, 
                buttonsContainer,
                () => StateMachine.SwitchState(MainMenuUIManager.StoreState), 
                "store-button")
            {
                text = TextData.StoreButton
            };

            StyledButton optionsButton = new(SelectedTheme.PlayerTheme, 
                buttonsContainer, 
                () => StateMachine.SwitchState(MainMenuUIManager.SettingsState), 
                "settings-button")
            {
                text = TextData.SettingsButton
            };
            
            StyledButton achievementsButton = new(SelectedTheme.PlayerTheme, 
                buttonsContainer, 
                () => StateMachine.SwitchState(MainMenuUIManager.AchievementsState), 
                "achievements-button")
            {
                text = TextData.AchievementsButton
            };
        }
    }
}