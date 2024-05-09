using Audio;
using EventBus;

namespace Events
{
    public class OnAudioSwitchValueChanged : IEvent
    {
        public readonly AudioType AudioType;
        public readonly bool Value;

        public OnAudioSwitchValueChanged(AudioType audioType, bool value)
        {
            AudioType = audioType;
            Value = value;
        }
    }
}