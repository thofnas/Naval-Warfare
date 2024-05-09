using System;
using EventBus;
using Events;
using FMOD.Studio;
using FMODUnity;
using Utilities.Extensions;

namespace Audio
{
    public class VolumeAdjuster
    {
        private VolumeAdjuster()
        {
            var onAudioSwitchValueChanged = new EventBinding<OnAudioSwitchValueChanged>(OnAudioSwitchValueChanged);
            EventBus<OnAudioSwitchValueChanged>.Register(onAudioSwitchValueChanged);
        }

        private static void OnAudioSwitchValueChanged(OnAudioSwitchValueChanged e)
        {
            string vcaPath = $"vca:/{Enum.GetName(e.AudioType.GetType(), e.AudioType)}";
            VCA vca = RuntimeManager.GetVCA(vcaPath);
            vca.setVolume(e.Value.ToInt());
        }
    }
}