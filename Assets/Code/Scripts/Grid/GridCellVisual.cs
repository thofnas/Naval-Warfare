using Themes;
using UnityEngine;
using Zenject;

namespace Grid
{
    [DisallowMultipleComponent]
    public class GridCellVisual : MonoBehaviour
    {
        private static readonly int s_outlineColor = Shader.PropertyToID("_OutlineColor");
        
        [SerializeField] private SpriteRenderer _gridCellFrame;
        [SerializeField] private SpriteRenderer _hitOrMissIcon;
        private GridCell _gridCell;
        private Color _defaultColor;
        private ThemeSettings _themeSettings;

        [Inject]
        public void Construct(GridCell gridCell, Vector2 position, Transform parent, ThemeSettings themeSettings, Sprite gridCellSprite)
        {
            _gridCell = gridCell;
            _themeSettings = themeSettings;
            _defaultColor = themeSettings.GridCellSpriteColor;
            _gridCellFrame.color = themeSettings.GridCellSpriteColor;
            _gridCellFrame.sprite = gridCellSprite;
            _gridCellFrame.material.SetColor(s_outlineColor, themeSettings.OutlineColor);
            _hitOrMissIcon.material.SetColor(s_outlineColor, themeSettings.OutlineColor);

            transform.position = position;
            transform.SetParent(parent);
        }

        public void UpdateFrameSprite(bool isEnemy = false)
        {
            Color frameColor;

            if (_gridCell.HasShip())
            {
                if (_gridCell.Ship.IsDestroyed())
                {
                    frameColor = _themeSettings.GridCellSpriteDestroyedShipColor;
                }
                else
                {
                    frameColor = isEnemy ? _defaultColor : _themeSettings.GridCellSpritePlacingColor;
                }
            }
            else if (_gridCell.IsSelected)
            {
                frameColor = _themeSettings.GridCellSpritePlacingColor;
            }
            else
            {
                frameColor = _defaultColor;
            }

            _gridCellFrame.color = frameColor;
        }


        public void UpdateIconSprite()
        {
            if (!_gridCell.WasHit)
            {
                _hitOrMissIcon.sprite = null;
                return;
            }
            
            _hitOrMissIcon.sprite = _gridCell.HasShip()
                ? _themeSettings.GridCellIcons[CellIcon.ShipHit]
                : _themeSettings.GridCellIcons[CellIcon.Miss];
        }

        public class Factory : PlaceholderFactory<GridCell, Vector2, Transform, ThemeSettings, Sprite, GridCellVisual>
        {
        }
    }
}