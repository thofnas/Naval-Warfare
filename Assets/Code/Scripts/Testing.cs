using UnityEngine;

[DisallowMultipleComponent]
public class Testing : MonoBehaviour
{
    [SerializeField] private Transform _circleTestPrefab;

    private void Start()
    {
    }

    private void Update()
    {
        //print( Level.Instance.GetValidGridCellPosition(Level.PlayerType.Player,MouseWorld2D.GetPosition(), new Vector2Int(2,4)));
    }
}