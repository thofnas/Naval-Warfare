using System;
using Enemy;
using EventBus;
using Events;
using UI;
using UI.Elements;
using UniRx;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
using Utilities.Extensions;

namespace States.GameplayUIStates
{
    public class BattleResults : BaseState
    {
        private const int TimeToActivateNavigationButtonsInMs = 500;
        
        private readonly Wallet _wallet;
        private readonly IDifficulty _difficulty;
        
        private Label _totalMoneyLabel;
        private Label _winLoseLabel;
        private StyledButton _goToMainMenuButton;
        private StyledButton _restartButton;
        private IDisposable _buttonsActivator;
        private VisualElement _resultsMoneyContainer;

        public BattleResults(GameplayUIManager gameplayUIManager, StyleSheet styleSheet, Wallet wallet, IDifficulty difficulty) : base(gameplayUIManager)
        {
            _wallet = wallet;
            _difficulty = difficulty;
            
            Root = CreateDocument(nameof(BattleResults), styleSheet);

            GenerateView();

            var onAllCharactersShipsDestroyed =
                new EventBinding<OnAllCharactersShipsDestroyed>(OnAllCharactersShipsDestroyed);
            EventBus<OnAllCharactersShipsDestroyed>.Register(onAllCharactersShipsDestroyed);

            var onMoneyChanged = new EventBinding<OnMoneyChanged>(OnMoneyChanged);
            EventBus<OnMoneyChanged>.Register(onMoneyChanged);
        }

        public sealed override void GenerateView()
        {
            VisualElement container = Root.CreateChild("container");
            
            VisualElement totalMoneyContainer = container.CreateChild("money-container");
            VisualElement moneyIconTotal = totalMoneyContainer.CreateChild("money-icon");
            
            _totalMoneyLabel = totalMoneyContainer.CreateChild<Label>("money-text");
            _totalMoneyLabel.text = _wallet.GetCurrentMoney().ToString();
            
            VisualElement containerResults = container.CreateChild("container-results");
            
            _winLoseLabel = containerResults.CreateChild<Label>();
            
            _resultsMoneyContainer = container.CreateChild("money-container-results"); 
            _resultsMoneyContainer.visible = false;
            _resultsMoneyContainer.CreateChild("money-icon");
            Label moneyResultsLabel = _resultsMoneyContainer.CreateChild<Label>();
            moneyResultsLabel.text = _difficulty.GetWinMoneyAmount().ToString();
            
            VisualElement containerButtons = container.CreateChild("container-buttons");

            _restartButton = new StyledButton(GameplayUIManager.Theme, containerButtons)
            {
                text = "Restart"
            };

            _goToMainMenuButton = new StyledButton(GameplayUIManager.Theme, containerButtons)
            {
                text = "Main menu"
            };
        }

        protected sealed override VisualElement Root { get; }
        
        public override void OnEnter()
        {
            base.OnEnter();

            const float dimAlphaValue = 0.8f;
            const int durationMs = 1000;
            Root.experimental.animation.Start(0f, dimAlphaValue, durationMs,
                (element, value) => { element.style.backgroundColor = Color.clear.With(a: value); });

            _buttonsActivator = Observable.Timer(TimeSpan.FromMilliseconds(TimeToActivateNavigationButtonsInMs))
                .Subscribe(_ => SetButtonEvents());
        }

        public override void OnExit()
        {
            base.OnEnter();

            Root.style.backgroundColor = Color.clear;
            
            _buttonsActivator.Dispose();
        } 

        private void SetButtonEvents()
        {
            _restartButton.clicked += () => SceneManager.LoadScene("Gameplay");
            _goToMainMenuButton.clicked += () => SceneManager.LoadScene("MainMenu");
        }

        private void OnMoneyChanged(OnMoneyChanged e)
        {
            _totalMoneyLabel.text = e.To.ToString();
        }

        private void OnAllCharactersShipsDestroyed(OnAllCharactersShipsDestroyed e)
        {
            if (e.LoserCharacterType == CharacterType.Enemy)
            {
                _winLoseLabel.text = "You won";

                _resultsMoneyContainer.visible = true;
            }
            else
                _winLoseLabel.text = "You lost";
        }
    }
}