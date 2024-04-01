using EventBus;
using Events;
using UI;
using UI.Elements;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
using Utilities.Extensions;

namespace States.GameplayUIStates
{
    public class BattleResults : BaseState
    {
        private Label _resultsLabel;

        public BattleResults(GameplayUIManager gameplayUIManager, StyleSheet styleSheet) : base(gameplayUIManager)
        {
            Root = CreateDocument(nameof(BattleResults), styleSheet);

            GenerateView();

            var onAllCharactersShipsDestroyed =
                new EventBinding<OnAllCharactersShipsDestroyed>(OnAllCharactersShipsDestroyed);
            EventBus<OnAllCharactersShipsDestroyed>.Register(onAllCharactersShipsDestroyed);
        }

        protected sealed override VisualElement Root { get; }
        
        public override void OnEnter()
        {
            base.OnEnter();

            const float dimAlphaValue = 0.8f;
            const int durationMs = 1000;
            Root.experimental.animation.Start(0f, dimAlphaValue, durationMs,
                (element, value) => { element.style.backgroundColor = Color.clear.With(a: value); });
        }
        
        public override void OnExit()
        {
            base.OnEnter();

            Root.style.backgroundColor = Color.clear;
        } 

        public sealed override void GenerateView()
        {
            VisualElement container = Root.CreateChild("container");
            VisualElement containerResults = container.CreateChild("container-results");
            VisualElement containerButtons = container.CreateChild("container-buttons");
            _resultsLabel = new Label();
            containerResults.Add(_resultsLabel);

            StyledButton restartButton = new(GameplayUIManager.ThemeSettings, containerButtons,
                () => SceneManager.LoadScene("Gameplay"))
            {
                text = "Restart"
            };

            StyledButton goToMainMenuButton = new(GameplayUIManager.ThemeSettings, containerButtons,
                () => SceneManager.LoadScene("MainMenu"))
            {
                text = "Main menu"
            };
        }

        private void OnAllCharactersShipsDestroyed(OnAllCharactersShipsDestroyed obj) =>
            _resultsLabel.text = obj.LostCharacterType == CharacterType.Enemy
                ? "You won"
                : "You lost";
    }
}