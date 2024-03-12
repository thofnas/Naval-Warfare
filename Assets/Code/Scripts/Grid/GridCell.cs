using EventBus;
using Events;
using UnityEngine;

namespace Grid
{
    public class GridCell
    {
        public Ship.Ship Ship { get; private set; }
        public bool WasHit { get; private set; }
        public bool IsSelected { get; private set; }
        public CellPosition CellPosition { get; }

        private readonly CharacterType _characterType;
        private readonly EventBinding<OnShipMoved> _onShipMoved;
        private readonly EventBinding<OnShoot> _onShootBinding;
        private readonly EventBinding<OnGridCellSelected> _onGridCellSelected;

        public GridCell(CharacterType characterType, CellPosition cellPosition)
        {
            CellPosition = cellPosition;
            _characterType = characterType;

            _onShootBinding = new EventBinding<OnShoot>(Player_OnShoot);
            _onShipMoved = new EventBinding<OnShipMoved>(Ship_OnMoved);
            _onGridCellSelected = new EventBinding<OnGridCellSelected>(GridCell_OnSelected);

            EventBus<OnShoot>.Register(_onShootBinding);
            EventBus<OnShipMoved>.Register(_onShipMoved);
            EventBus<OnGridCellSelected>.Register(_onGridCellSelected);
        }

        public void Dispose()
        {
            EventBus<OnShoot>.Deregister(_onShootBinding);
            EventBus<OnShipMoved>.Deregister(_onShipMoved);
            EventBus<OnGridCellSelected>.Deregister(_onGridCellSelected);
        }

        public bool HasShip() => Ship != null;

        public override string ToString() =>
            HasShip()
                ? $"{CellPosition}\n{Ship}"
                : CellPosition.ToString();

        private void SetShip(Ship.Ship ship) => Ship = ship;

        private void ClearShip() => Ship = null;

        private bool TryApplyHit()
        {
            if (WasHit) return false;

            WasHit = true;

            EventBus<OnCellHit>.Invoke(new OnCellHit(_characterType, CellPosition, Ship));

            return true;
        }

        private void Player_OnShoot(OnShoot e)
        {
            if (e.CellPosition == CellPosition && e.Receiver == _characterType)
                TryApplyHit();
        }

        private void Ship_OnMoved(OnShipMoved e)
        {
            if (e.CharacterType != _characterType) return;
            
            foreach (CellPosition cellPosition in e.From)
                if (cellPosition == CellPosition)
                    ClearShip();

            foreach (CellPosition cellPosition in e.To)
                if (cellPosition == CellPosition)
                    SetShip(e.Ship);
        }

        private void GridCell_OnSelected(OnGridCellSelected e)
        {
            if (e.CharacterType != _characterType) return;
            
            if (e.From == CellPosition) IsSelected = false;
            
            if (e.To == CellPosition) IsSelected = true;
        }
    }
}