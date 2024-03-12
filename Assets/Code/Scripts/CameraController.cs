using System;
using System.Linq;
using DG.Tweening;
using EventBus;
using Events;
using Grid;
using UnityEngine;
using Utilities.Extensions;
using Zenject;

public class CameraController : IInitializable, ITickable, IDisposable
{    
    private const int MaxOrthographicSize = 7;
    private const float FloatTolerance = 0.05f;
    
    private static Vector2[] s_halvesCenterPositions;
    
    private readonly GridSystem.Settings _gridSystemSettings;
    private float _previousOrthographicSize;
    private EventBinding<OnBattleStateEntered> _onBattleStateEntered;

    
    [Inject]
    private CameraController(GridSystem.Settings gridSystemSettings)
    {
        _gridSystemSettings = gridSystemSettings;
        Camera.main.orthographicSize = Mathf.Min(MaxOrthographicSize, Mathf.Max(_gridSystemSettings.Height, _gridSystemSettings.Width) + 1 * _gridSystemSettings.CellGap);
        SetHalvesCenterPositions(Camera.main);
    }

    public void Initialize()
    {
        Vector2 playerGridFirstPosition = s_halvesCenterPositions.First();
        Camera.main.transform.position = Camera.main.transform.position.With(playerGridFirstPosition.x, playerGridFirstPosition.y);

        _onBattleStateEntered = new EventBinding<OnBattleStateEntered>(() =>
            Camera.main.transform.DOMove(Vector3.zero.With(z: Camera.main.transform.position.z), 1f));
        EventBus<OnBattleStateEntered>.Register(_onBattleStateEntered);
    }


    public void Tick()
    {
        if (Math.Abs(Camera.main.orthographicSize - _previousOrthographicSize) < FloatTolerance) return;
        
        EventBus<OnCameraOrthographicSizeChanged>.Invoke(new OnCameraOrthographicSizeChanged());
        _previousOrthographicSize = Camera.main.orthographicSize;
    }

    public void Dispose() =>
        EventBus<OnBattleStateEntered>.Deregister(_onBattleStateEntered);

    public static Vector2[] GetHalvesCenterPositions() => s_halvesCenterPositions;

    private void SetHalvesCenterPositions(Camera camera)
    {
        if (!camera.orthographic)
        {
            Debug.LogWarning("GetHalvesCenter is designed for orthographic cameras.");
            return;
        }

        float orthographicSize = camera.orthographicSize;
        const float aspectRatio = 1920f / 1080f; //1.777778

        float halfWidth = orthographicSize * aspectRatio * 0.5f;

        Vector3 center = camera.transform.position;

        var halvesCenters = new Vector2[2];
        halvesCenters[0] = new Vector2(center.x - halfWidth, center.y); // Left half
        halvesCenters[1] = new Vector2(center.x + halfWidth, center.y); // Right half

        s_halvesCenterPositions = halvesCenters;
    }
}