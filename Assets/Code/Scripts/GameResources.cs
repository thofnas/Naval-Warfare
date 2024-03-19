using Grid;
using Themes;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UIElements;

[DisallowMultipleComponent]
public class GameResources : MonoBehaviour
{
    private static GameResources s_instance;

    public GridCellVisual GridCellVisualPrefab => _gridCellVisualPrefab;
    public Ship.Ship ShipPrefab => _shipPrefab;
    public UIDocument UIDocumentPrefab => _uiDocumentPrefab;
    public Transform ShipHitExplosionPrefab => _shipHitExplosionPrefab;
    public SpriteRenderer BackgroundPrefab => _backgroundPrefab;
    public Theme AITheme => _aiTheme;

    [SerializeField] private GridCellVisual _gridCellVisualPrefab;
    [SerializeField] private Ship.Ship _shipPrefab;
    [SerializeField] private UIDocument _uiDocumentPrefab;
    [SerializeField] private Transform _shipHitExplosionPrefab;   
    [SerializeField] private SpriteRenderer _backgroundPrefab;
    [FormerlySerializedAs("_aiThemeSettings")] [SerializeField] private Theme _aiTheme;

    public static GameResources Instance
    {
        // We get data from a prefab
        // With this we can easily share data between scenes, cause everything is located in resource folder

        get
        {
            if (s_instance == null)
                s_instance = Resources.Load<GameResources>(nameof(GameResources));

            return s_instance;
        }
    }
}