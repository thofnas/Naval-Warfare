using Misc;
using Newtonsoft.Json;
// ReSharper disable InconsistentNaming

namespace Data
{
    public class TextData
    {
        public readonly string Title;
        public readonly string PlayButton;
        public readonly string StoreButton;
        public readonly string SettingsButton;
        public readonly string AchievementsButton;
        public readonly string BackButton;
        public readonly string FramesPerSecond;
        public readonly string Language;
        public readonly string Unlocked;
        public readonly string Audio;
        public readonly string Music;
        public readonly string FirstMapBought_Name;
        public readonly string FirstMapBought_UnlockCondition;
        public readonly string WinTenTimes_Name;
        public readonly string WinTenTimes_UnlockCondition;
 

        [JsonConstructor]
        public TextData(string title, string playButton, string storeButton,
            string settingsButton, string achievementsButton, string backButton,
            string framesPerSecond, string language, string unlocked, 
            string firstMapBought_Name, string firstMapBought_UnlockCondition, string winTenTimes_Name, 
            string winTenTimes_UnlockCondition, string audio, string music)
        {
            Title = title;
            PlayButton = playButton;
            StoreButton = storeButton;
            SettingsButton = settingsButton;
            AchievementsButton = achievementsButton;
            BackButton = backButton;
            FramesPerSecond = framesPerSecond;
            Language = language;
            Unlocked = unlocked;
            FirstMapBought_Name = firstMapBought_Name;
            FirstMapBought_UnlockCondition = firstMapBought_UnlockCondition;
            WinTenTimes_Name = winTenTimes_Name;
            WinTenTimes_UnlockCondition = winTenTimes_UnlockCondition;
            Audio = audio;
            Music = music;
        }
    }
}