using TMPro;
using UnityEngine;

namespace Grid
{
    [DisallowMultipleComponent]
    public class GridCellDebug : MonoBehaviour
    {
        [SerializeField] private TextMeshPro _textMeshPro;
        private GridCell _cell;

        private void Update() => _textMeshPro.text = _cell.ToString();

        public void SetGridCell(GridCell cell) => _cell = cell;
    }
}