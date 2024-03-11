using System.Collections.Generic;
using Grid;
using Scripts.EventBus;

namespace Events
{
    public class OnShipPlacementPreviewMoved : IEvent
    {
        public readonly List<CellPosition> From;
        public readonly Ship.Ship Ship;
        public readonly List<CellPosition> To;

        public OnShipPlacementPreviewMoved(Ship.Ship ship, List<CellPosition> from, List<CellPosition> to)
        {
            Ship = ship;
            From = from;
            To = to;
        }
    }
}