using EventBus;
using Events;
using UI;
using UI.Elements;
using UnityEngine;
using UnityEngine.UIElements;
using Utilities.Extensions;

namespace States.GameplayUIStates
{
    public class PlacingShips : BaseState
    {
        private VisualElement _container;

        private Label _countDownLabel;
        private Toggle _readyToggle;
        private StyledButton _styledButton;
        private EventBinding<OnCountdownUpdated> _onCountdownUpdated;
        private EventBinding<OnBattleStateEntered> _onBattleStateEntered;

        public PlacingShips(GameplayUIManager gameplayUIManager, StyleSheet styleSheet) : base(gameplayUIManager)
        {
            Root = CreateDocument(nameof(PlacingShips), styleSheet);
            
            GenerateView();
        }

        protected sealed override VisualElement Root { get; }


        public sealed override void GenerateView()
        {
            _container = Root.CreateChild("container");
            VisualElement center = _container.CreateChild("countdown-container");
            VisualElement buttons = _container.CreateChild("buttons-container");

            _styledButton = new StyledButton(GameplayUIManager.Theme, buttons, "randomize-btn")
            {
                text = "Randomize"
            };
            _readyToggle = new StyledToggle(GameplayUIManager.Theme, buttons, "ready-toggle")
            {
                text = "Ready"
            };
            _countDownLabel = new Label("1") { visible = false };

            center.CreateChild("countdown-text").Add(_countDownLabel);
        }

        public override void OnEnter()
        {
            base.OnEnter();

            _onCountdownUpdated = new EventBinding<OnCountdownUpdated>(Countdown_OnSecondPassed);
            EventBus<OnCountdownUpdated>.Register(_onCountdownUpdated);

            _readyToggle.RegisterCallback<MouseUpEvent>(_ => ReadyToggle_OnValueChanged(_readyToggle.value));

            _styledButton.clicked += () => EventBus<OnRandomizeButtonClicked>.Invoke(new OnRandomizeButtonClicked());
        }

        public override void OnExit()
        {
            base.OnExit();

            EventBus<OnCountdownUpdated>.Deregister(_onCountdownUpdated);
            EventBus<OnBattleStateEntered>.Deregister(_onBattleStateEntered);
            _readyToggle.UnregisterCallback<MouseUpEvent>(_ => ReadyToggle_OnValueChanged(_readyToggle.value));
        }

        private void Countdown_OnSecondPassed(OnCountdownUpdated e)
        {
            Vector3 targetPosition = _container.transform.position + Vector3.left * 10;
            Vector3 startPosition = _container.transform.position + Vector3.right * 10;

            _countDownLabel.text = e.Seconds.ToString();
            _countDownLabel.experimental.animation.Position(targetPosition, 1000).from = startPosition;
            _countDownLabel.experimental.animation
                    .Start(0, 1, 500, (element, value) =>
                        element.style.opacity = new StyleFloat(value))
                    .onAnimationCompleted +=
                () => _countDownLabel.experimental.animation
                    .Start(1, 0, 500, (element, value) =>
                        element.style.opacity = new StyleFloat(value));
        }

        private void ReadyToggle_OnValueChanged(bool isOn)
        {
            _countDownLabel.visible = isOn;
            EventBus<OnReadyUIButtonToggled>.Invoke(new OnReadyUIButtonToggled(_readyToggle));
        }
    }
}