using System;
using EventBus;
using Events;

public class TurnSystem : IDisposable
{
    private readonly EventBinding<OnCellHit> _onShipHitBinding;

    private readonly CharacterType _whoseFirstTurn;
    private bool _isPlacingShips;
    private CharacterType _whoseCurrentTurn;

    public TurnSystem(CharacterType whoseFirstTurn)
    {
        _whoseFirstTurn = whoseFirstTurn;
        _whoseCurrentTurn = whoseFirstTurn;
        _isPlacingShips = true;

        _whoseCurrentTurn = CharacterType.Player;

        _onShipHitBinding = new EventBinding<OnCellHit>(NextTurn);

        EventBus<OnCellHit>.Register(_onShipHitBinding);
    }

    public int TurnCount { get; private set; }

    public void Dispose() => EventBus<OnCellHit>.Deregister(_onShipHitBinding);

    public void NextTurn()
    {
        if (_isPlacingShips)
        {
            CompletePlacing();
            return;
        }

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

    public bool IsPlacingShips() => _isPlacingShips;

    public CharacterType WhoseCurrentTurn() => _whoseCurrentTurn;

    public CharacterType WhoWillTakeAHit() =>
        _whoseCurrentTurn == CharacterType.Enemy
            ? CharacterType.Player
            : CharacterType.Enemy;

    private void CompletePlacing() => _isPlacingShips = false;
}