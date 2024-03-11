using System;
using System.Collections.Generic;
using Grid;
using UnityEngine;
using Utilities.Extensions;
using Zenject;

public class ShipsSpawner
{
    private Settings _settings;
    private Ship.Ship.Factory _shipFactory;

    [Inject]
    private void Construct(Ship.Ship.Factory shipFactory, Settings settings)
    {
        _shipFactory = shipFactory;
        _settings = settings;
    }

    public void SpawnShipsFor(GridSystem gridSystem)
    {
        var shipsGameObject = new GameObject("Ships");
        shipsGameObject.SetParent(gridSystem.Parent);

        foreach (ShipData data in _settings.ShipData)
        {
            Ship.Ship ship = _shipFactory.Create();
            ship.transform.SetParent(shipsGameObject.transform);

            ship.Init(gridSystem.GetCharacterType(), data.ShipLength);
        }
    }

    [Serializable]
    public class Settings
    {
        public List<ShipData> ShipData = new();
    }

    [Serializable]
    public class ShipData
    {
        [Min(1)] public int ShipLength = 2;
    }
}