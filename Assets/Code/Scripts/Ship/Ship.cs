using System.Collections.Generic;
using System.Linq;
using Events;
using Grid;
using Scripts.EventBus;
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
        private CharacterType _characterType;
        private GameManager _gameManager;
        private int _health;
        private bool _isDestroyed;
        private bool _isHorizontal;
        private Level _level;
        private Vector3 _mouseOffset;
        private List<CellPosition> _occupiedCellPositions = new();

        private EventBinding<OnCellHit> _onShipHitBinding;
        private BoxCollider2D _shipCollider;
        private int _shipLength;
        private ShipVisual _shipVisual;

        private void OnEnable()
        {
            _onShipHitBinding = new EventBinding<OnCellHit>(Ship_OnHit);
            EventBus<OnCellHit>.Register(_onShipHitBinding);
        }

        private void OnDisable() => EventBus<OnCellHit>.Deregister(_onShipHitBinding);

        private void OnMouseDown()
        {
            if (!CanDragAndPlace()) return;

            _mouseOffset = MouseWorld2D.GetPosition() - transform.position;
        }

        private void OnMouseDrag()
        {
            if (!CanDragAndPlace()) return;

            if (Input.GetKeyDown(KeyCode.R)) TryRotate();

            Vector3 mousePos = MouseWorld2D.GetPosition();
            transform.position = mousePos - _mouseOffset;
        }

        private void OnMouseUp()
        {
            if (!CanDragAndPlace()) return;

            if (!_level.TryGetValidGridCellPositions(_characterType, transform.position, this,
                    out List<CellPosition> cellPositions)) return;

            TrySetNewShipPositions(cellPositions);
        }

        [Inject]
        private void Construct(GameManager gameManager, Level level)
        {
            _gameManager = gameManager;
            _level = level;
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

        public int GetShipLength() => _shipLength;

        public bool TrySetNewShipPositions(List<CellPosition> newCellPositions)
        {
            if (!_level.CanPlaceShipAt(_characterType, newCellPositions[0], this))
                return false;

            transform.position = (Vector3)_level.GetWorldCellPosition(_characterType, newCellPositions[0]) +
                                 GetSpriteOffset();

            EventBus<OnShipMoved>.Invoke(
                new OnShipMoved(this, _occupiedCellPositions, newCellPositions));

            _occupiedCellPositions = newCellPositions;

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

            if (!_level.TryGetValidGridCellPositions(_characterType, transform.position, this,
                    out List<CellPosition> cellPositions))
            {
                _isHorizontal = !_isHorizontal;
                return false;
            }

            TrySetNewShipPositions(cellPositions);

            _shipVisual.UpdateSprite();
            _shipVisual.UpdatePlacementSprite();

            Quaternion rotation = transform.rotation;
            rotation.eulerAngles = rotation.eulerAngles.With(z: GetZRotation());
            transform.rotation = rotation;

            return true;
        }

        public bool CanDragAndPlace()
        {
            if (_characterType == CharacterType.Enemy)
                return false;

            if (_gameManager.CountdownTimer.IsActive())
                return false;

            if (!_gameManager.IsCurrentState(_gameManager.PlacingShips))
                return false;

            return true;
        }

        public bool IsDestroyed() => _isDestroyed;

        public bool HasDamagedCell(out CellPosition damagedCellPosition)
        {
            // search for damaged cells
            foreach (CellPosition cellPosition in _occupiedCellPositions.Where(cellPosition =>
                         _level.WasGridCellHit(_characterType, cellPosition)))
            {
                damagedCellPosition = cellPosition;
                return true;
            }

            damagedCellPosition = new CellPosition(0, 0);
            return false;
        }

        public Vector3 GetSpriteOffset() =>
            IsHorizontal()
                ? new Vector3(_level.GetCellSize(_characterType) * (_shipLength - 1) * 0.5f, 0f, 0f)
                : new Vector3(0f, _level.GetCellSize(_characterType) * (_shipLength - 1) * 0.5f, 0f);

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