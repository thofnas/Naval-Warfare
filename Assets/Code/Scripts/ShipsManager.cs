using System;
using System.Collections.Generic;
using System.Linq;
using EventBus;
using Events;
using Grid;
using Zenject;
using Random = UnityEngine.Random;

public class ShipsManager : IInitializable, IDisposable
{
    private Dictionary<CharacterType, List<Ship.Ship>> _allShips;
    private LevelManager _levelManager;
    private EventBinding<OnRandomizeButtonClicked> _onRandomizeButtonClicked;
    private EventBinding<OnShipDestroyed> _shipDestroyedEventBinding;
    private EventBinding<OnShipSpawned> _shipSpawnedEventBinding;

    [Inject]
    private void Construct(LevelManager levelManager)
    {
        _levelManager = levelManager;

        _allShips = new Dictionary<CharacterType, List<Ship.Ship>>
        {
            { CharacterType.Player, new List<Ship.Ship>() },
            { CharacterType.Enemy, new List<Ship.Ship>() }
        };

        _shipSpawnedEventBinding = new EventBinding<OnShipSpawned>(Ship_OnSpawned);
        _shipDestroyedEventBinding = new EventBinding<OnShipDestroyed>(Ship_OnDestroyed);
        _onRandomizeButtonClicked = new EventBinding<OnRandomizeButtonClicked>(RandomizeButton_OnClick);
        EventBus<OnShipSpawned>.Register(_shipSpawnedEventBinding);
        EventBus<OnShipDestroyed>.Register(_shipDestroyedEventBinding);
        EventBus<OnRandomizeButtonClicked>.Register(_onRandomizeButtonClicked);
    }

    public void Initialize()
    {
        foreach (KeyValuePair<CharacterType, List<Ship.Ship>> allShipsKeyValue in _allShips)
        {
            foreach (Ship.Ship ship in allShipsKeyValue.Value)
            {
                PlaceShipsOnGrid(ship, allShipsKeyValue.Key);
            }
            
            RandomizeShips(allShipsKeyValue.Value);
        }
    }

    public void Dispose()
    {
        EventBus<OnShipSpawned>.Deregister(_shipSpawnedEventBinding);
        EventBus<OnShipDestroyed>.Deregister(_shipDestroyedEventBinding);
        EventBus<OnRandomizeButtonClicked>.Deregister(_onRandomizeButtonClicked);
    }

    public List<Ship.Ship> GetShips(CharacterType characterType) => _allShips[characterType];

    public bool HasUndestroyedShip()
    {
        foreach (Ship.Ship ship in _allShips[CharacterType.Player])
        {
            if (ship.IsDestroyed()) continue;
            if (!ship.HasDamagedCell(out CellPosition cell)) continue;

            return true;
        }

        return false;
    }

    public void RandomizeShips(List<Ship.Ship> ships)
    {
        foreach (Ship.Ship ship in ships)
        {
            ship.TrySetNewShipPositions(_levelManager.GetValidRandomCellPositions(ship));
            if (Random.value > 0.5f) ship.TryRotate();
        }
    }
    
    private void PlaceShipsOnGrid(Ship.Ship ship, CharacterType characterType)
    {
        for (var x = 0; x < _levelManager.GetGridSystemWidth(characterType); x++)
        for (var y = 0; y < _levelManager.GetGridSystemHeight(characterType); y++)
        {
            if (!_levelManager.TryGetValidGridCellPositions(characterType,
                    _levelManager.GetWorldCellPosition(characterType, new CellPosition(x, y)), ship,
                    out List<CellPosition> validCellPositions))
                continue;

            if (ship.TrySetNewShipPositions(validCellPositions))
                return; // Move to the next ship if placed successfully
        }
    }

    private bool AreAllShipsDestroyed(CharacterType characterType)
    {
        if (!_allShips.TryGetValue(characterType, out List<Ship.Ship> ships)) return false;

        return ships.All(ship => ship.IsDestroyed());
    }

    private void Ship_OnSpawned(OnShipSpawned e) => _allShips[e.CharacterType].Add(e.Ship);

    private void Ship_OnDestroyed(OnShipDestroyed e)
    {
        CharacterType destroyedShipCharacterType = e.CharacterType;

        if (AreAllShipsDestroyed(destroyedShipCharacterType))
            EventBus<OnAllCharactersShipsDestroyed>.Invoke(
                new OnAllCharactersShipsDestroyed(destroyedShipCharacterType));
    }

    private void RandomizeButton_OnClick() => RandomizeShips(GetShips(CharacterType.Player));
}