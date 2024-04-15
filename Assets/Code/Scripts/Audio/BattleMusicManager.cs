using System;
using System.Collections.Generic;
using System.Data;
using EventBus;
using Events;
using FMOD.Studio;
using FMODUnity;
using States.GameplayStates;
using UnityEngine;
using Zenject;
using STOP_MODE = FMOD.Studio.STOP_MODE;

namespace Audio
{
    public class BattleMusicManager : IInitializable, IDisposable
    {
        private const uint StateParamIndex = 0;
        
        private readonly Map.Map _selectedMap;
        private EventInstance _musicInstance;

        private EventBinding<OnGameplayStateChanged> _onGameplayStateChanged;
        private EventDescription _eventDescription;
        private PARAMETER_DESCRIPTION _parameterDescription;

        private BattleMusicManager(Map.Map selectedMap)
        {
            _selectedMap = selectedMap;
        }

        public void Initialize()
        {
            _onGameplayStateChanged = new EventBinding<OnGameplayStateChanged>(e => SetParameter(e.NewState));
            EventBus<OnGameplayStateChanged>.Register(_onGameplayStateChanged);
            
            if (_selectedMap.Music.IsNull) return;
            
            _musicInstance = RuntimeManager.CreateInstance(_selectedMap.Music);
            _musicInstance.getDescription(out EventDescription eventDescription);
            _eventDescription = eventDescription;
            
            eventDescription.getParameterDescriptionCount(out int paramCount);
            
            eventDescription.getParameterDescriptionByIndex((int) StateParamIndex, out PARAMETER_DESCRIPTION parameterDescription);
            _parameterDescription = parameterDescription;
            
            eventDescription.loadSampleData();
            _musicInstance.start();
            
            if (StateParamIndex > paramCount - 1)
                throw new ArgumentOutOfRangeException(nameof(paramCount), paramCount, "StateParam index is bigger than param count");

            CheckParameterDescription(parameterDescription);
        }

        public void Dispose()
        {
            EventBus<OnGameplayStateChanged>.Deregister(_onGameplayStateChanged);
            
            _musicInstance.stop(STOP_MODE.ALLOWFADEOUT);
            _musicInstance.release();
        }

        private void SetParameter(Type gameplayStateType)
        {
            if (_selectedMap.Music.IsNull) return;
            
            CheckParameterDescription(_parameterDescription);
            
            int paramIndex = TypeIndexMapper.GetIndex(gameplayStateType);
            
            PARAMETER_ID parameterID = _parameterDescription.id;
            _eventDescription.getParameterLabelByID(parameterID, paramIndex, out string label);
            
            _musicInstance.setParameterByIDWithLabel(parameterID, label);
        }

        private static void CheckParameterDescription(PARAMETER_DESCRIPTION parameterDescription)
        {
            if (parameterDescription.guid == Guid.Empty)
                throw new NoNullAllowedException("Parameter description is null.");
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