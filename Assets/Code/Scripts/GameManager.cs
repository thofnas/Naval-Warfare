using System;
using EventBus;
using Events;
using Misc;
using StateMachine;
using States.GameplayStates;
using Utilities;
using Zenject;

public class GameManager : IInitializable, IDisposable
{
    private const int CountdownSeconds = 1;
    private const float CountdownWaitSecondsBeforeComplete = 0.95f;
    private readonly ShipsManager _shipsManager;
    private readonly StateMachine.StateMachine _stateMachine;

    private EventBinding<OnAllCharactersShipsDestroyed> _onAllCharactersShipsDestroyed;
    private EventBinding<OnReadyUIButtonToggled> _readyToggleBinding;

    [Inject]
    private GameManager(StateMachine.StateMachine stateMachine, Level level)
    {
        _stateMachine = stateMachine;

        CountdownTimer = new CountdownTimer(CountdownSeconds, CountdownWaitSecondsBeforeComplete);

        // creating states
        PlacingShips = new PlacingShips(stateMachine);
        Battle = new Battle(level, stateMachine, () => IsBattleEnded = true);
        BattleResults = new BattleResults(stateMachine);

        _stateMachine.SetState(PlacingShips);
    }

    public CountdownTimer CountdownTimer { get; }
    public bool IsBattleEnded { get; private set; }
    
    // states
    public PlacingShips PlacingShips { get; }
    public Battle Battle { get; }
    public BattleResults BattleResults { get; }

    public void Dispose()
    {
        EventBus<OnAllCharactersShipsDestroyed>.Deregister(_onAllCharactersShipsDestroyed);
        EventBus<OnReadyUIButtonToggled>.Deregister(_readyToggleBinding);
    }

    public void Initialize()
    {
        _onAllCharactersShipsDestroyed = new EventBinding<OnAllCharactersShipsDestroyed>(Ships_OnOneSideDestroyed);
        EventBus<OnAllCharactersShipsDestroyed>.Register(_onAllCharactersShipsDestroyed);
        _readyToggleBinding = new EventBinding<OnReadyUIButtonToggled>(ReadyToggle_OnClick);
        EventBus<OnReadyUIButtonToggled>.Register(_readyToggleBinding);
    }

    public bool IsCurrentState(IState state) => _stateMachine.IsCurrentState(state);
    
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
                _stateMachine.SwitchState(Battle);
            });
    }

    private void Ships_OnOneSideDestroyed(OnAllCharactersShipsDestroyed obj)
    {
        _stateMachine.SwitchState(BattleResults);
    }
}