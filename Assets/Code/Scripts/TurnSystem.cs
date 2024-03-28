using System;
using EventBus;
using Events;

public class TurnSystem : IDisposable
{
    private readonly GameManager _gameManager;
    private readonly CharacterType _whoseFirstTurn;
    private CharacterType _whoseCurrentTurn;
    
    private readonly EventBinding<OnCellHit> _onShipHit;
    private readonly EventBinding<OnBattleStateEntered> _onBattleStateEntered;

    public TurnSystem(CharacterType whoseFirstTurn, GameManager gameManager)
    {
        _gameManager = gameManager;
        _whoseFirstTurn = whoseFirstTurn;
        _whoseCurrentTurn = whoseFirstTurn;

        _whoseCurrentTurn = CharacterType.Player;

        _onShipHit = new EventBinding<OnCellHit>(NextTurn);
        _onBattleStateEntered = new EventBinding<OnBattleStateEntered>(NextTurn);
        
        EventBus<OnCellHit>.Register(_onShipHit);
        EventBus<OnBattleStateEntered>.Register(_onBattleStateEntered);
    }

    public void Dispose()
    {
        EventBus<OnCellHit>.Deregister(_onShipHit);
        EventBus<OnBattleStateEntered>.Deregister(_onBattleStateEntered);
    }

    private int TurnCount { get; set; }

    private void NextTurn()
    {
        if (IsPlacingShips())
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

    public bool IsPlacingShips() => _gameManager.IsCurrentState(_gameManager.PlacingShips);

    public CharacterType WhoseCurrentTurn() => _whoseCurrentTurn;

    public CharacterType WhoWillTakeAHit() =>
        _whoseCurrentTurn == CharacterType.Enemy
            ? CharacterType.Player
            : CharacterType.Enemy;
}