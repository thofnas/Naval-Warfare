using System;
using EventBus;
using Events;
using FMOD.Studio;
using FMODUnity;
using Zenject;

namespace Audio
{
    public class UISoundsManager : IInitializable, IDisposable
    {
        private readonly UISoundLibrary _uiSoundLibrary;
        private EventDescription _buttonClickDescription;
        private EventBinding<OnButtonClick> _onButtonClick;
        
        private UISoundsManager(UISoundLibrary uiSoundLibrary)
        {
            _uiSoundLibrary = uiSoundLibrary;
        }

        public void Initialize()
        {
            _buttonClickDescription = RuntimeManager.GetEventDescription(_uiSoundLibrary.ButtonClick);
            _buttonClickDescription.loadSampleData();

            _onButtonClick = new EventBinding<OnButtonClick>(OnButtonClick);
            EventBus<OnButtonClick>.Register(_onButtonClick);
        }

        private void OnButtonClick(OnButtonClick e)
        {
            if (e.IsSwitch)
                PlayOneShot(_uiSoundLibrary.SwitchButtonClick);
            else
                PlayOneShot(_uiSoundLibrary.ButtonClick);
        }

        public void Dispose()
        {
            _buttonClickDescription.unloadSampleData();
            
            EventBus<OnButtonClick>.Deregister(_onButtonClick);
        }
        
        private static void PlayOneShot(EventReference sound) => 
            RuntimeManager.PlayOneShot(sound);
    }
}