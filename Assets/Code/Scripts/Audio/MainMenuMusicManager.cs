using System;
using FMOD.Studio;
using FMODUnity;
using Zenject;
using STOP_MODE = FMOD.Studio.STOP_MODE;

namespace Audio
{
    public class MainMenuMusicManager : IInitializable, IDisposable
    {
        private readonly EventReference _music;
        private EventInstance _musicInstance;

        public MainMenuMusicManager(EventReference music)
        {
            _music = music;
        }
        
        public void Initialize()
        {
            _musicInstance = RuntimeManager.CreateInstance(_music);
            _musicInstance.start();
        }

        public void Dispose()
        {
            _musicInstance.stop(STOP_MODE.ALLOWFADEOUT);
            _musicInstance.release();
        }
    }
}