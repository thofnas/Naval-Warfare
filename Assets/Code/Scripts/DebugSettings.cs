using System;
using System.Collections.Generic;
using Grid;
using UnityEngine;
using VInspector;

[Serializable]
public class DebugSettings
{
    [Tooltip("Starts from 0. x0 y0 will be the cell in the left-bottom corner")]
    public List<CellPosition.SerializableData> FirstCellPositionsAIWillShoot;

    [Header("Debug")] public bool CreateDebugObjects;

    [ShowIf(nameof(CreateDebugObjects))] public GridCellDebug GridDebugObjectPrefab;
}