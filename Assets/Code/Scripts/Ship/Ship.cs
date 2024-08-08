using System.Collections.Generic;
using System.Linq;
using EventBus;
using Events;
using GameplayStates;
using Grid;
using UnityEngine;
using Utilities;
using Utilities.Extensions;
using Zenject;

namespace Ship
{
    [RequireComponent(typeof(ShipVisual))]
    [DisallowMultipleComponent]
    public class Ship : MonoBehaviour
    {
        public IEnumerable<CellPosition> OccupiedCellPositions => _occupiedCellPositions;
        
        private ShipVisual _shipVisual;
        private CharacterType _characterType;
        private int _health;
        private bool _isDestroyed;
        private bool _isHorizontal;
        private int _shipLength;
        private bool _wasPlacementPreviewMoved;
        private Vector3 _positionBeforeMoving;
        private bool _wasTransformMovedBeyondTolerance;
        private Vector3 _mouseOffset;
        private LevelManager _levelManager;
        private List<CellPosition> _occupiedCellPositions = new();
        private BoxCollider2D _shipCollider;
        private GameplayManager _gameplayManager;

        private EventBinding<OnCellHit> _onCellHitBinding;
        private EventBinding<OnShipPlacementPreviewMoved> _onPlacementPreviewMoved;

        [Inject]
        private void Construct(GameplayManager gameplayManager, LevelManager levelManager)
        {
            _gameplayManager = gameplayManager;
            _levelManager = levelManager;
        }

        public Ship Init(CharacterType characterType, int shipLength)
        {
            _characterType = characterType;
            _shipLength = shipLength;
            _health = shipLength;
            _shipCollider = GetComponent<BoxCollider2D>();
            _shipCollider.size = _shipCollider.size.With(y: shipLength);

            _shipVisual = GetComponent<ShipVisual>();

            _shipVisual.Init(this);

            EventBus<OnShipSpawned>.Invoke(new OnShipSpawned(this, _characterType));

            return this;
        }

        private void OnEnable()
        {
            _onCellHitBinding = new EventBinding<OnCellHit>(Ship_OnHit);
            EventBus<OnCellHit>.Register(_onCellHitBinding);
        }

        private void OnDisable() => EventBus<OnCellHit>.Deregister(_onCellHitBinding);

        private void OnMouseDown()
        {
            if (!CanDragAndPlace()) return;

            Vector3 position = transform.position;
            
            _mouseOffset = MouseWorld2D.GetPosition() - position;
            _positionBeforeMoving = position;
            
            _wasPlacementPreviewMoved = false;
            _wasTransformMovedBeyondTolerance = false;
            
            _onPlacementPreviewMoved = new EventBinding<OnShipPlacementPreviewMoved>(_ => _wasPlacementPreviewMoved = true);
            EventBus<OnShipPlacementPreviewMoved>.Register(_onPlacementPreviewMoved);
        }

        private void OnMouseDrag()
        {
            if (!CanDragAndPlace()) return;

            if (!_wasTransformMovedBeyondTolerance)
            {
                const float tolerance = 0.25f;
                if ((_positionBeforeMoving - transform.position).magnitude > tolerance)
                {
                    EventBus<OnShipGrabStatusChanged>.Invoke(new OnShipGrabStatusChanged(ship: this, isGrabbing: true, _characterType));
                    _wasTransformMovedBeyondTolerance = true;
                }
            }
            else
            {
                _shipVisual.HideInteractButtons();
            }

            Vector3 mousePos = MouseWorld2D.GetPosition();
            transform.position = mousePos - _mouseOffset;
        }

        private void OnMouseUp()
        {
            if (!CanDragAndPlace()) return;

            if (!_levelManager.TryGetValidGridCellPositions(_characterType, transform.position, this,
                    out List<CellPosition> cellPositions))
            {
                transform.position = (Vector3)_levelManager.GetWorldCellPosition(_characterType, _occupiedCellPositions[0]) +
                                     GetSpriteOffset();
                return;
            }

            if (!_wasPlacementPreviewMoved)
                if (_occupiedCellPositions.ToHashSet().SetEquals(cellPositions.ToHashSet()))
                    _shipVisual.ShowInteractButtons();
            
            if (_wasTransformMovedBeyondTolerance) 
                EventBus<OnShipGrabStatusChanged>.Invoke(new OnShipGrabStatusChanged(ship: this, isGrabbing: false, _characterType));

            TrySetNewShipPositions(cellPositions);
            
            EventBus<OnShipPlacementPreviewMoved>.Deregister(_onPlacementPreviewMoved);
        }

        public int GetShipLength() => _shipLength;

        public bool TrySetNewShipPositions(List<CellPosition> newCellPositions)
        {
            if (!_levelManager.CanPlaceShipAt(_characterType, newCellPositions[0], this))
                return false;

            transform.position = (Vector3)_levelManager.GetWorldCellPosition(_characterType, newCellPositions[0]) +
                                 GetSpriteOffset();

            EventBus<OnShipMoved>.Invoke(
                new OnShipMoved(this, _occupiedCellPositions, newCellPositions));
            
            EventBus<OnShipPlacementPreviewMoved>.Invoke(
                new OnShipPlacementPreviewMoved(this, _occupiedCellPositions, newCellPositions));

            _occupiedCellPositions = newCellPositions;
            
            _shipVisual.UpdatePlacementPreviewSprite();

            return true;
        }

        public bool IsHorizontal() => _isHorizontal;

        public CharacterType GetCharacterType() => _characterType;

        public Vector2Int GetShipDimensions() =>
            _isHorizontal
                ? new Vector2Int(_shipLength, 1)
                : new Vector2Int(1, _shipLength);

        public bool TryRotate()
        {
            _isHorizontal = !_isHorizontal;

            if (!_levelManager.TryGetValidGridCellPositions(_characterType, transform.position, this,
                    out List<CellPosition> cellPositions))
            {
                _isHorizontal = !_isHorizontal;
                return false;
            }

            if (TrySetNewShipPositions(cellPositions))
                _shipVisual.HideInteractButtons();

            _shipVisual.UpdateSprite();

            Quaternion rotation = transform.rotation;
            rotation.eulerAngles = rotation.eulerAngles.With(z: GetZRotation());
            transform.rotation = rotation;

            return true;
        }

        public bool CanDragAndPlace()
        {
            if (_characterType == CharacterType.Enemy)
                return false;

            if (_gameplayManager.CountdownTimer.IsActive)
                return false;

            if (!_gameplayManager.IsCurrentState(typeof(PlacingShips)))
                return false;

            return true;
        }

        public bool IsDestroyed() => _isDestroyed;

        public bool HasDamagedCell(out CellPosition damagedCellPosition)
        {
            // search for damaged cells
            foreach (CellPosition cellPosition in _occupiedCellPositions.Where(cellPosition =>
                         _levelManager.WasGridCellHit(_characterType, cellPosition)))
            {
                damagedCellPosition = cellPosition;
                return true;
            }

            damagedCellPosition = new CellPosition(0, 0);
            return false;
        }

        public Vector3 GetSpriteOffset() =>
            IsHorizontal()
                ? new Vector3(_levelManager.GetCellSize(_characterType) * (_shipLength - 1) * 0.5f, 0f, 0f)
                : new Vector3(0f, _levelManager.GetCellSize(_characterType) * (_shipLength - 1) * 0.5f, 0f);

        public float GetZRotation() =>
            IsHorizontal()
                ? 90f
                : 0f;

        private void Ship_OnHit(OnCellHit e)
        {
            if (e.WoundedCharacterType != GetCharacterType()) return;
            if (e.Ship != this) return;
            if (!_occupiedCellPositions.Contains(e.HitCellPosition)) return;

            _health--;

            if (_health > 0) return;
            if (_isDestroyed) return;

            _isDestroyed = true;
            EventBus<OnShipDestroyed>.Invoke(new OnShipDestroyed(this, _characterType, _occupiedCellPositions));
        }

        public class Factory : PlaceholderFactory<Ship>
        {
        }
    }
}