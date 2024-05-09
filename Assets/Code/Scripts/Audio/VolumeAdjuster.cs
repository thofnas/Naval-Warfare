using System;
using EventBus;
using Events;
using FMOD.Studio;
using FMODUnity;
using UnityEngine;
using Utilities.Extensions;

namespace Audio
{
    public class VolumeAdjuster
    {
        private VolumeAdjuster()
        {
            var onAudioSwitchValueChanged = new EventBinding<OnAudioSwitchValueChanged>(OnAudioSwitchValueChanged);
            EventBus<OnAudioSwitchValueChanged>.Register(onAudioSwitchValueChanged);

            VCA musicVca = RuntimeManager.GetVCA($"vca:/{nameof(AudioType.BGM)}");
            musicVca.setVolume(GameSettings.IsMusicEnabled().ToInt());
            
            VCA sfxVca = RuntimeManager.GetVCA($"vca:/{nameof(AudioType.Sfx)}");
            sfxVca.setVolume(GameSettings.IsSfxEnabled().ToInt());
        }

        private static void OnAudioSwitchValueChanged(OnAudioSwitchValueChanged e)
        {
            string vcaPath = $"vca:/{Enum.GetName(e.AudioType.GetType(), e.AudioType)}";
            VCA vca = RuntimeManager.GetVCA(vcaPath);
            vca.setVolume(e.Value.ToInt());
        }
    }
}