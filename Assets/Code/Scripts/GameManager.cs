using System;
using Events;
using Scripts.EventBus;
using StateMachine;
using States.GameplayStates;
using Utilities;
using Zenject;

public class GameManager : ITickable, IInitializable, IDisposable
{
    private const int CountdownSeconds = 1;
    private const float CountdownWaitSecondsBeforeComplete = 0.95f;
    private readonly ShipsManager _shipsManager;
    private readonly StateMachine.StateMachine _stateMachine;
    private bool _areShipsOnOneSideDestroyed;

    private bool _isReadyCountdownCompleted;
    private EventBinding<OnAllCharactersShipsDestroyed> _onAllCharactersShipsDestroyed;
    private EventBinding<OnReadyUIButtonToggled> _readyToggleBinding;

    [Inject]
    private GameManager(StateMachine.StateMachine stateMachine, TurnSystem turnSystem, Level level)
    {
        _stateMachine = stateMachine;

        CountdownTimer = new CountdownTimer(CountdownSeconds, CountdownWaitSecondsBeforeComplete);

        // creating states
        PlacingShips = new PlacingShips(turnSystem);
        Battle = new Battle(turnSystem, level, () => WasBattleEnded = true);
        BattleResults = new BattleResults(turnSystem);

        At(PlacingShips, Battle, new FuncPredicate(() => _isReadyCountdownCompleted));
        At(Battle, BattleResults, new FuncPredicate(() => _areShipsOnOneSideDestroyed));

        _stateMachine.SetState(PlacingShips);
    }

    public CountdownTimer CountdownTimer { get; }
    public bool WasBattleEnded { get; private set; }


    // states
    public PlacingShips PlacingShips { get; }
    public Battle Battle { get; }
    public BattleResults BattleResults { get; }

    public void Dispose()
    {
        EventBus<OnReadyUIButtonToggled>.Deregister(_readyToggleBinding);
        EventBus<OnAllCharactersShipsDestroyed>.Deregister(_onAllCharactersShipsDestroyed);
    }

    public void Initialize()
    {
        _readyToggleBinding = new EventBinding<OnReadyUIButtonToggled>(ReadyToggle_OnClick);
        _onAllCharactersShipsDestroyed = new EventBinding<OnAllCharactersShipsDestroyed>(Ships_OnOneSideDestroyed);
        EventBus<OnReadyUIButtonToggled>.Register(_readyToggleBinding);
        EventBus<OnAllCharactersShipsDestroyed>.Register(_onAllCharactersShipsDestroyed);
    }

    public void Tick() => _stateMachine?.Update();

    public bool IsCurrentState(IState state) => _stateMachine.IsCurrentState(state);

    private void At(IState from, IState to, IPredicate condition) => _stateMachine.AddTransition(from, to, condition);

    private void Any(IState to, IPredicate condition) => _stateMachine.AddAnyTransition(to, condition);


    private void ReadyToggle_OnClick(OnReadyUIButtonToggled e)
    {
        if (!IsCurrentState(PlacingShips)) return;

        // if toggle is OFF, just cancel it
        if (!e.IsOn)
        {
            CountdownTimer.CancelCountdown();
            return;
        }

        CountdownTimer.StartCountdown(
            second => { EventBus<OnCountdownUpdated>.Invoke(new OnCountdownUpdated(second)); },
            () =>
            {
                e.Toggle.value = false;
                _isReadyCountdownCompleted = true;
            });
    }

    private void Ships_OnOneSideDestroyed(OnAllCharactersShipsDestroyed obj) => _areShipsOnOneSideDestroyed = true;
}