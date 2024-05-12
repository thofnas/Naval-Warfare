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
        private StyledToggle _readyToggle;
        private StyledButton _randomizeButton;
        private EventBinding<OnCountdownUpdated> _onCountdownUpdated;
        private EventBinding<OnBattleStateEntered> _onBattleStateEntered;
        private EventBinding<OnShipGrabStatusChanged> _onShipGrabStatusChanged;

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

            _randomizeButton = new StyledButton(GameplayUIManager.Theme, buttons, "randomize-btn")
            {
                text = "Randomize"
            };
            _readyToggle = new StyledToggle(GameplayUIManager.Theme, buttons, "ready-toggle")
            {
                text = "Ready"
            };
            _countDownLabel = new Label("2") { visible = false };

            center.CreateChild("countdown-text").Add(_countDownLabel);
        }

        public override void OnEnter()
        {
            base.OnEnter();

            _onCountdownUpdated = new EventBinding<OnCountdownUpdated>(Countdown_OnSecondPassed);
            _onShipGrabStatusChanged = new EventBinding<OnShipGrabStatusChanged>(Ship_OnGrabStatusChanged);
            
            EventBus<OnCountdownUpdated>.Register(_onCountdownUpdated);
            EventBus<OnShipGrabStatusChanged>.Register(_onShipGrabStatusChanged);

            _readyToggle.RegisterValueChangedCallback(_ => ReadyToggle_OnValueChanged(_readyToggle.value));

            _randomizeButton.clicked += () => EventBus<OnRandomizeButtonClicked>.Invoke(new OnRandomizeButtonClicked());
        }

        public override void OnExit()
        {
            base.OnExit();

            EventBus<OnCountdownUpdated>.Deregister(_onCountdownUpdated);
            EventBus<OnBattleStateEntered>.Deregister(_onBattleStateEntered);
            EventBus<OnShipGrabStatusChanged>.Deregister(_onShipGrabStatusChanged);
            
            _readyToggle.UnregisterValueChangedCallback(_ => ReadyToggle_OnValueChanged(_readyToggle.value));
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

        private void Ship_OnGrabStatusChanged(OnShipGrabStatusChanged e)
        {
            const float isGrabbingOpacity = 0.4f;
            const float isNotGrabbingOpacity = 1f;
            const int durationMs = 200;
            
            if (e.IsGrabbing)
            {
                _randomizeButton.AnimateOpacity(isNotGrabbingOpacity, isGrabbingOpacity, durationMs);
                
                _readyToggle.AnimateOpacity(isNotGrabbingOpacity, isGrabbingOpacity, durationMs);
            }
            else
            {
                _randomizeButton.AnimateOpacity(isGrabbingOpacity, isNotGrabbingOpacity, durationMs);
            
                _readyToggle.AnimateOpacity(isGrabbingOpacity, isNotGrabbingOpacity, durationMs);
            }
        }
    }
}