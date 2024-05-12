using EventBus;
using UnityEngine;

namespace Events
{
    public struct OnShipGrabStatusChanged : IEvent
    {
        public readonly Ship.Ship Ship;
        public readonly bool IsGrabbing;
        private readonly CharacterType _characterType;

        public OnShipGrabStatusChanged(Ship.Ship ship, bool isGrabbing, CharacterType characterType)
        {
            Ship = ship;
            IsGrabbing = isGrabbing;
            _characterType = characterType;
        }
    }
}