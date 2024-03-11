using System.Collections.Generic;
using DG.Tweening;
using Events;
using Scripts.EventBus;
using Themes;
using UnityEngine;
using Zenject;

namespace Grid
{
    [DisallowMultipleComponent]
    public class GridSystemVisual : MonoBehaviour
    {
        private CharacterType _characterType;
        private GridCellVisual.Factory _gridCellVisualFactory;
        private GridCellVisual[,] _gridCellVisuals;
        private Vector2 _gridCenter;
        private GridSystem _gridSystem;
        private Transform _parent;
        private ThemeSettings _themeSettings;
        
        // Events
        private EventBinding<OnCellHit> _onHit;
        private EventBinding<OnGridCellSelected> _onNewGridCellSelected;
        private EventBinding<OnShipMoved> _onShipChangedPositionBinding;
        private EventBinding<OnShipDestroyed> _onShipDestroyed;
        private EventBinding<OnShipPlacementPreviewMoved> _onShipValidPlacementPositionsChanged;

        [Inject]
        private void Construct(GridSystem gridSystem, Vector3 battlePosition, ThemeSettings themeSettings,
            GridCellVisual.Factory gridCellVisualFactory)
        {
            _parent = gridSystem.Parent.transform;
            transform.SetParent(_parent);

            _themeSettings = themeSettings;
            _gridCellVisualFactory = gridCellVisualFactory;
            _gridSystem = gridSystem;
            _characterType = gridSystem.GetCharacterType();
            _gridCenter = CalculateGridCenter(gridSystem.Width, gridSystem.Height, gridSystem.CellSize);

            _gridCellVisuals = new GridCellVisual[
                gridSystem.Width,
                gridSystem.Height
            ];

            for (var x = 0; x < gridSystem.Width; x++)
            for (var y = 0; y < gridSystem.Height; y++)
            {
                GridCellVisual gridCellVisual =
                    _gridCellVisualFactory.Create(gridSystem.GetGridCell(new CellPosition(x, y)), 
                        GetWorldCellPosition(new CellPosition(x, y)), 
                        transform, 
                        themeSettings, 
                        GetCellSprite(x, y, themeSettings.GridCellSprites));

                _gridCellVisuals[x, y] = gridCellVisual;
            }
        }
        
        private Sprite GetCellSprite(int x, int y, IReadOnlyList<Sprite> gridSprites)
        {
            bool isLeft = x == 0;
            bool isRight = x == _gridSystem.Width - 1;
            bool isTop = y == 0;
            bool isBottom = y == _gridSystem.Height - 1;

            if (isLeft && isBottom) return gridSprites[0];  // left bottom corner
            if (isRight && isBottom) return gridSprites[2]; // right bottom corner
            if (isLeft && isTop) return gridSprites[6];     // left top corner
            if (isRight && isTop) return gridSprites[8];    // right top corner

            if (!isLeft && !isRight && isBottom) return gridSprites[1]; // bottom side
            if (!isLeft && !isRight && isTop) return gridSprites[7];    // top side
            if (isLeft && !isTop && !isBottom) return gridSprites[3];   // left side
            if (isRight && !isTop && !isBottom) return gridSprites[5];  // right side

            return gridSprites[4]; // center
        }

        private void OnEnable()
        {
            _onShipChangedPositionBinding = new EventBinding<OnShipMoved>(Ship_OnChangedPosition);
            _onShipValidPlacementPositionsChanged =
                new EventBinding<OnShipPlacementPreviewMoved>(Ship_OnPlacementPreviewPositionsChanged);
            _onHit = new EventBinding<OnCellHit>(GridCell_OnShipHit);
            _onShipDestroyed = new EventBinding<OnShipDestroyed>(Ship_OnDestroyed);
            _onNewGridCellSelected = new EventBinding<OnGridCellSelected>(GridCell_OnNewSelected);

            EventBus<OnShipMoved>.Register(_onShipChangedPositionBinding);
            EventBus<OnShipPlacementPreviewMoved>.Register(_onShipValidPlacementPositionsChanged);
            EventBus<OnCellHit>.Register(_onHit);
            EventBus<OnShipDestroyed>.Register(_onShipDestroyed);
            EventBus<OnGridCellSelected>.Register(_onNewGridCellSelected);
        }

        private void OnDisable()
        {
            EventBus<OnShipMoved>.Deregister(_onShipChangedPositionBinding);
            EventBus<OnShipPlacementPreviewMoved>.Deregister(_onShipValidPlacementPositionsChanged);
            EventBus<OnCellHit>.Deregister(_onHit);
            EventBus<OnShipDestroyed>.Deregister(_onShipDestroyed);
            EventBus<OnGridCellSelected>.Deregister(_onNewGridCellSelected);
        }

        public Vector2 GetWorldCellPosition(CellPosition cellPosition) =>
            new Vector2(cellPosition.x, cellPosition.y) * _gridSystem.CellSize + GetOffset();

        public Vector2 GetGridCenter() => _gridCenter;

        public void MoveTo(Vector2 position) => _parent.position = position - _gridCenter;

        public void MoveTo(Vector2 position, float duration) => _parent.DOMove(position - _gridCenter, duration);

        public Vector2 GetOffset() => _parent.position;

        public Sprite GetIcon(CellIcon cellIcon) => _themeSettings.GridCellIcons[cellIcon];

        private static Vector2 CalculateGridCenter(int width, int height, float cellSize) =>
            new(
                (width - 1) * cellSize * 0.5f,
                (height - 1) * cellSize * 0.5f
            );

        private void GridCell_OnNewSelected(OnGridCellSelected e)
        {
            if (e.CharacterType != _characterType) return;
            
            _gridCellVisuals[e.From.x, e.From.y].UpdateFrameSprite(isEnemy: e.CharacterType == CharacterType.Enemy);
            _gridCellVisuals[e.To.x, e.To.y].UpdateFrameSprite(isEnemy: e.CharacterType == CharacterType.Enemy);
        }

        private void Ship_OnChangedPosition(OnShipMoved e)
        {
            if (_characterType == CharacterType.Enemy) return;
            if (e.Ship.GetCharacterType() == CharacterType.Enemy) return;

            e.From.ForEach(position => _gridCellVisuals[position.x, position.y].UpdateFrameSprite());

            e.To.ForEach(position => _gridCellVisuals[position.x, position.y].UpdateFrameSprite());
        }

        private void Ship_OnPlacementPreviewPositionsChanged(OnShipPlacementPreviewMoved e)
        {
            if (_characterType == CharacterType.Enemy) return;
            if (e.Ship.GetCharacterType() == CharacterType.Enemy) return;

            e.From.ForEach(position => _gridCellVisuals[position.x, position.y].UpdateFrameSprite());

            e.To.ForEach(position => _gridCellVisuals[position.x, position.y].UpdateFrameSprite());
        }

        private void Ship_OnDestroyed(OnShipDestroyed e)
        {
            if (_characterType != e.CharacterType) return;

            foreach (CellPosition shipCellPosition in e.ShipCellPositions)
                _gridCellVisuals[shipCellPosition.x, shipCellPosition.y]
                    .UpdateFrameSprite();
        }

        private void GridCell_OnShipHit(OnCellHit e)
        {
            if (e.WoundedCharacterType != _characterType) return;

            _gridCellVisuals[e.HitCellPosition.x, e.HitCellPosition.y].UpdateIconSprite();
            _gridCellVisuals[e.HitCellPosition.x, e.HitCellPosition.y].UpdateFrameSprite();
        }

        public class Factory : PlaceholderFactory<GridSystem, Vector3, ThemeSettings, GridSystemVisual>
        {
        }
    }
}