﻿using System.Linq;
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
        private GridSystemVisual _gridSystemVisual;
        private GridCell _gridCell;
        private Color _defaultColor;
        private Theme _theme;
        private MaterialPropertyBlock _materialPropertyBlock;

        [Inject]
        public void Construct(GridSystemVisual gridSystemVisual, GridCell gridCell, Vector2 position, Transform parent, Theme theme, Sprite gridCellSprite)
        {
            _gridSystemVisual = gridSystemVisual;
            _gridCell = gridCell;
            _theme = theme;
            _defaultColor = theme.GridCellSpriteColor;
            _gridCellFrame.color = theme.GridCellSpriteColor;
            _gridCellFrame.sprite = gridCellSprite;

            _materialPropertyBlock = new MaterialPropertyBlock();
            _materialPropertyBlock.SetColor(s_outlineColor, theme.OutlineColor);
            _gridCellFrame.SetPropertyBlock(_materialPropertyBlock);
            _hitOrMissIcon.SetPropertyBlock(_materialPropertyBlock);

            transform.position = position;
            transform.SetParent(parent);
        }
        
        
        public void UpdateFrameSpriteColor(bool isEnemyGrid = false)
        {
            Color frameColor;
            
            if (_gridCell.IsSelected)
            {
                frameColor = _theme.GridCellSpritePlacingColor;
            }
            else if (_gridSystemVisual.IsGrabbingAShip)
            {
                frameColor = _gridSystemVisual.PlacementPreviewPositions.Any(cellPosition =>
                    cellPosition == _gridCell.CellPosition)
                    ? _theme.GridCellSpritePlacingColor
                    : _defaultColor;
            }
            else if (_gridCell.HasShip())
            {
                if (_gridCell.Ship.IsDestroyed())
                {
                    frameColor = _theme.GridCellSpriteDestroyedShipColor;
                }
                else
                {
                    frameColor = isEnemyGrid ? _defaultColor : _theme.GridCellSpritePlacingColor;
                }
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
                ? _theme.GridCellIcons[CellIcon.ShipHit]
                : _theme.GridCellIcons[CellIcon.Miss];
        }

        public class Factory : PlaceholderFactory<GridSystemVisual, GridCell, Vector2, Transform, Theme, Sprite, GridCellVisual>
        {
        }
    }
}