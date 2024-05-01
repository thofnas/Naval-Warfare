using System;
using Events;
using FMODUnity;
using UnityEngine;
using Zenject;
using EventBus;
using FMOD.Studio;

namespace Audio
{
    public class BattleSfxManager : IInitializable, IDisposable
    {
        private readonly Map.Map _selectedMap;
        private readonly LevelManager _levelManager;
        private EventBinding<OnCellHit> _onCellHit;
        private EventDescription _shipHitEventDescription;
        private EventDescription _shipMissedEventDescription;

        private BattleSfxManager(Map.Map selectedMap, LevelManager levelManager)
        {
            _selectedMap = selectedMap;
            _levelManager = levelManager;
        }

        public void Initialize()
        {
            _shipHitEventDescription = RuntimeManager.GetEventDescription(_selectedMap.ShipHitSound);
            _shipMissedEventDescription = RuntimeManager.GetEventDescription(_selectedMap.ShipMissedSound);
            _shipHitEventDescription.loadSampleData();
            _shipMissedEventDescription.loadSampleData();
            
            _onCellHit = new EventBinding<OnCellHit>(OnCellHit);
            EventBus<OnCellHit>.Register(_onCellHit);
        }

        public void Dispose()
        {
            _shipHitEventDescription.unloadSampleData();
            _shipMissedEventDescription.unloadSampleData();
            
            EventBus<OnCellHit>.Deregister(_onCellHit);
        }

        private static void PlayOneShot(EventReference sound, Vector2 worldPosition) => 
            RuntimeManager.PlayOneShot(sound, worldPosition);

        private void OnCellHit(OnCellHit e)
        {
            Vector2 position = _levelManager.GetWorldCellPosition(e.WoundedCharacterType, e.HitCellPosition);

            PlayOneShot(e.Ship == null
                ? _selectedMap.ShipMissedSound
                : _selectedMap.ShipHitSound, position);
        }
    }
}