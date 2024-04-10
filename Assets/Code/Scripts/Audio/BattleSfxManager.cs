﻿using System;
using Events;
using FMODUnity;
using UnityEngine;
using Zenject;
using EventBus;

namespace Audio
{
    public class BattleSfxManager : IInitializable, IDisposable
    {
        private readonly Map.Map _selectedMap;
        private readonly LevelManager _levelManager;

        private BattleSfxManager(Map.Map selectedMap, LevelManager levelManager)
        {
            _selectedMap = selectedMap;
            _levelManager = levelManager;
        }

        public void Initialize()
        {
            EventBus<OnCellHit>.Register(new EventBinding<OnCellHit>(OnShipHit));
        }

        public void Dispose()
        {
            EventBus<OnCellHit>.Deregister(new EventBinding<OnCellHit>(OnShipHit));
        }

        private static void PlayOneShot(EventReference sound, Vector2 worldPosition) => 
            RuntimeManager.PlayOneShot(sound, worldPosition);

        private void OnShipHit(OnCellHit e)
        {
            if (e.Ship == null) return;
            
            Vector2 position = _levelManager.GetWorldCellPosition(e.WoundedCharacterType, e.HitCellPosition);
            
            PlayOneShot(_selectedMap.ShipHitSound, position);
        }
    }
}