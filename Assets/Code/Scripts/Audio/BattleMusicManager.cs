using System;
using FMOD.Studio;
using FMODUnity;
using Zenject;
using STOP_MODE = FMOD.Studio.STOP_MODE;

namespace Audio
{
    public class BattleMusicManager : IInitializable, IDisposable
    {
        private readonly Map.Map _selectedMap;
        private EventInstance _musicInstance;

        private BattleMusicManager(Map.Map selectedMap)
        {
            _selectedMap = selectedMap;
        }

        public void Initialize()
        {
            _musicInstance = RuntimeManager.CreateInstance(_selectedMap.Music);
            _musicInstance.start();
        }

        public void Dispose()
        {
            _musicInstance.stop(STOP_MODE.ALLOWFADEOUT);
            _musicInstance.release();
        }
    }
}