using System;
using AI;
using Data;
using EventBus;
using Events;
using GameplayStates;
using StateMachine;
using Utilities;
using Zenject;

public class GameplayManager : IInitializable, IDisposable
{
    public CharacterType LoserCharacterType { get; private set; }
    
    private const int CountdownSeconds = 2;
    private const float CountdownWaitSecondsBeforeComplete = 0.95f;
    private readonly ShipsManager _shipsManager;
    private readonly StateMachine.StateMachine _stateMachine;

    private EventBinding<OnAllCharactersShipsDestroyed> _onAllCharactersShipsDestroyed;
    private EventBinding<OnReadyUIButtonToggled> _readyToggleBinding;

    [Inject]
    private GameplayManager(StateMachine.StateMachine stateMachine, LevelManager levelManager, Wallet wallet, IDifficulty difficulty)
    {
        _stateMachine = stateMachine;

        CountdownTimer = new CountdownTimer(CountdownSeconds, CountdownWaitSecondsBeforeComplete);

        // creating states
        PlacingShips = new PlacingShips(stateMachine);
        Battle = new Battle(levelManager, stateMachine, () => IsBattleEnded = true);
        BattleResults = new BattleResults(this, stateMachine, wallet, difficulty);
            
        _stateMachine.OnStateChanged += StateMachine_OnStateChanged;

        _stateMachine.SetState(PlacingShips);
    }

    public CountdownTimer CountdownTimer { get; }
    public bool IsBattleEnded { get; private set; }
    
    // states
    public PlacingShips PlacingShips { get; }
    public Battle Battle { get; }
    public BattleResults BattleResults { get; }

    public void Initialize()
    {
        _onAllCharactersShipsDestroyed = new EventBinding<OnAllCharactersShipsDestroyed>(Ships_OnOneSideDestroyed);
        EventBus<OnAllCharactersShipsDestroyed>.Register(_onAllCharactersShipsDestroyed);
        
        _readyToggleBinding = new EventBinding<OnReadyUIButtonToggled>(ReadyToggle_OnClick);
        EventBus<OnReadyUIButtonToggled>.Register(_readyToggleBinding);
    }

    public void Dispose()
    {
        EventBus<OnAllCharactersShipsDestroyed>.Deregister(_onAllCharactersShipsDestroyed);
        EventBus<OnReadyUIButtonToggled>.Deregister(_readyToggleBinding);
        
        _stateMachine.OnStateChanged -= StateMachine_OnStateChanged;
    }

    public bool IsCurrentState(Type stateType) => _stateMachine.IsCurrentState(stateType);
    
    private void ReadyToggle_OnClick(OnReadyUIButtonToggled e)
    {
        if (!IsCurrentState(PlacingShips.GetType())) return;

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

    private void Ships_OnOneSideDestroyed(OnAllCharactersShipsDestroyed e)
    {
        LoserCharacterType = e.LoserCharacterType;
        _stateMachine.SwitchState(BattleResults);
    }

    private void StateMachine_OnStateChanged(IState from, IState to) => 
        EventBus<OnGameplayStateChanged>.Invoke(new OnGameplayStateChanged(from as BaseState, to as BaseState));
}