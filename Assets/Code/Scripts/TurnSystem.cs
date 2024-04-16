using System;
using EventBus;
using Events;
using States.GameplayStates;
using UnityEngine;

public class TurnSystem : IDisposable
{
    private readonly GameplayManager _gameplayManager;
    private readonly CharacterType _whoseFirstTurn;
    private CharacterType _whoseCurrentTurn;
    
    private readonly EventBinding<OnCellHit> _onShipHit;

    public TurnSystem(CharacterType whoseFirstTurn, GameplayManager gameplayManager)
    {
        _gameplayManager = gameplayManager;
        _whoseFirstTurn = whoseFirstTurn;
        _whoseCurrentTurn = whoseFirstTurn;

        _whoseCurrentTurn = CharacterType.Player;

        _onShipHit = new EventBinding<OnCellHit>(NextTurn);
        
        EventBus<OnCellHit>.Register(_onShipHit);
    }

    public void Dispose()
    {
        EventBus<OnCellHit>.Deregister(_onShipHit);
    }

    private int TurnCount { get; set; }

    private void NextTurn()
    {
        if (IsPlacingShips())
            return;
        
        if (_gameplayManager.IsCurrentState(typeof(BattleResults)))
            return;

        TurnCount++;
        _whoseCurrentTurn = _whoseCurrentTurn == CharacterType.Player
            ? CharacterType.Enemy
            : CharacterType.Player;
        EventBus<OnTurnChanged>.Invoke(new OnTurnChanged(_whoseCurrentTurn));
    }

    public int GetCharactersTurnCount(CharacterType characterType)
    {
        // Check if the total turn count is odd and the provided characterType is the one who starts first.
        if (TurnCount % 2 == 1 && characterType == _whoseFirstTurn)
            // If true, return (TurnCount / 2) + 1 turns for the starting character.
            return TurnCount / 2 + 1;

        // If false or if the characterType is not the one who starts first, return TurnCount / 2 turns.
        return TurnCount / 2;
    }

    public bool IsPlacingShips() => _gameplayManager.IsCurrentState(typeof(PlacingShips));

    public CharacterType WhoseCurrentTurn() => _whoseCurrentTurn;

    public CharacterType WhoWillTakeAHit() =>
        _whoseCurrentTurn == CharacterType.Enemy
            ? CharacterType.Player
            : CharacterType.Enemy;
}