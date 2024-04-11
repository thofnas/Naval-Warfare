using System;
using System.Collections.Generic;
using EventBus;
using Events;
using FMOD.Studio;
using FMODUnity;
using Map;
using States.GameplayStates;
using UnityEngine;
using Zenject;
using STOP_MODE = FMOD.Studio.STOP_MODE;

namespace Audio
{
    public class BattleMusicManager : IInitializable, IDisposable
    {
        
        private readonly Map.Map _selectedMap;
        private EventInstance _musicInstance;

        private EventBinding<OnGameplayStateChanged> _onGameplayStateChanged;

        private BattleMusicManager(Map.Map selectedMap)
        {
            _selectedMap = selectedMap;
        }

        public void Initialize()
        {
            _onGameplayStateChanged = new EventBinding<OnGameplayStateChanged>(e => SetParameter(e.NewState));
            EventBus<OnGameplayStateChanged>.Register(_onGameplayStateChanged);
            
            _musicInstance = RuntimeManager.CreateInstance(_selectedMap.Music);
            _musicInstance.getDescription(out EventDescription eventDescription);
            eventDescription.loadSampleData();
            _musicInstance.start();
        }

        public void Dispose()
        {
            EventBus<OnGameplayStateChanged>.Deregister(_onGameplayStateChanged);
            
            _musicInstance.stop(STOP_MODE.ALLOWFADEOUT);
            _musicInstance.release();
        }

        private void SetParameter(Type gameplayStateType)
        {
            // TODO: When FMOD updates, uncomment this, and change param's initial value to "PlacingShips" in FMOD Studio
            
            // int paramIndex = TypeIndexMapper.GetIndex(gameplayStateType);
            //
            // _musicInstance.getDescription(out EventDescription eventDescription);
            // eventDescription.getParameterDescriptionByIndex(paramIndex, out PARAMETER_DESCRIPTION parameterDescription);
            //
            // PARAMETER_ID parameterID = parameterDescription.id;
            // eventDescription.getParameterLabelByID(parameterID, paramIndex, out string label);
            
            // Crashes on this line
            // _musicInstance.setParameterByIDWithLabel(parameterID, label);
        }

        private static class TypeIndexMapper
        {
            private const int PlacingShipsParamIndex = 0;
            private const int BattleParamIndex = 1;
            private const int BattleResultsParamIndex = 2;
        
            private static readonly Dictionary<Type, int> s_mapTypeToThemeType = new()
            {
                { typeof(PlacingShips), PlacingShipsParamIndex },
                { typeof(Battle), BattleParamIndex },
                { typeof(BattleResults), BattleResultsParamIndex }
            };

            public static int GetIndex (Type type) => s_mapTypeToThemeType[type];
        }
    }
}