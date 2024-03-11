using StateMachine;
using States.GameplayUIStates;
using Themes;
using UnityEngine;
using UnityEngine.UIElements;
using Zenject;

namespace UI
{
    [DisallowMultipleComponent]
    public class GameplayUIManager : MonoBehaviour
    {
        [SerializeField] private StyleSheet _placingShipsStyleSheet;
        [SerializeField] private StyleSheet _battleStyleSheet;
        [SerializeField] private StyleSheet _battleResultsStyleSheet;
        private GameManager _gameManager;
        private StateMachine.StateMachine _stateMachine;
        public ThemeSettings ThemeSettings { get; private set; }

        private void Awake()
        {
            // defining states
            var placingShips = new PlacingShips(this, _placingShipsStyleSheet);
            var battle = new Battle(this, _battleStyleSheet);
            var battleResults = new BattleResults(this, _battleResultsStyleSheet);

            // creating transitions
            At(placingShips, battle, new FuncPredicate(() => _gameManager.IsCurrentState(_gameManager.Battle)));
            At(battle, battleResults, new FuncPredicate(() => _gameManager.IsCurrentState(_gameManager.BattleResults)));

            // set first state
            _stateMachine.SetState(placingShips);
        }

        private void Update() => _stateMachine.Update();

        [Inject]
        private void Construct(GameManager gameManager, Level level, StateMachine.StateMachine stateMachine,
            ThemeSettings themeSettings)
        {
            _gameManager = gameManager;
            _stateMachine = stateMachine;
            ThemeSettings = themeSettings;
        }

        private void At(IState from, IState to, IPredicate condition) =>
            _stateMachine.AddTransition(from, to, condition);

        private void Any(IState to, IPredicate condition) => _stateMachine.AddAnyTransition(to, condition);
    }
}