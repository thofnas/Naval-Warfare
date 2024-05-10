using System;
using FMOD.Studio;
using FMODUnity;
using UnityEngine.SceneManagement;
using Zenject;
using STOP_MODE = FMOD.Studio.STOP_MODE;

namespace Audio
{
    public class MainMenuMusicManager : IInitializable, IDisposable
    {
        private readonly EventReference _music;
        private EventInstance _musicInstance;

        private MainMenuMusicManager(EventReference music)
        {
            _music = music;
        }
        
        public void Initialize()
        {
            SceneManager.activeSceneChanged += SceneManager_OnActiveSceneChanged;
            
            _musicInstance = RuntimeManager.CreateInstance(_music);
            _musicInstance.start();
        }

        private void SceneManager_OnActiveSceneChanged(Scene current, Scene next)
        {
            if (next.name == SceneManager.GetSceneByBuildIndex(0).name) 
            {
                _musicInstance.start();
            }
            else
            {
                _musicInstance.stop(STOP_MODE.IMMEDIATE);
            }
        }

        public void Dispose()
        {
            _musicInstance.stop(STOP_MODE.ALLOWFADEOUT);
            _musicInstance.release();
        }
    }
}