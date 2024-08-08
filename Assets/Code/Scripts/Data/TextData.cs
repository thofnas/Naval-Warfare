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
        public readonly string UnlimitedButton;
        public readonly string ReadyToggle;
        public readonly string RandomizeButton;
        public readonly string BattleResults_YouWon;
        public readonly string BattleResults_YouLost;
        public readonly string BattleResults_Restart;
        public readonly string BattleResults_MainMenuButton;
        public readonly string FramesPerSecond;
        public readonly string Language;
        public readonly string Unlocked;
        public readonly string Audio;
        public readonly string Music;
        public readonly string Sfx;
        public readonly string UI;
        public readonly string FirstMapBought_Name;
        public readonly string FirstMapBought_UnlockCondition;
        public readonly string WinTenTimes_Name;
        public readonly string WinTenTimes_UnlockCondition;
        public readonly string WinFiveTimes_Name;
        public readonly string WinFiveTimes_UnlockCondition;


        [JsonConstructor]
        public TextData(string title, string playButton, string storeButton,
            string settingsButton, string achievementsButton, string backButton,
            string framesPerSecond, string language, string unlocked,
            string firstMapBought_Name, string firstMapBought_UnlockCondition, string winTenTimes_Name,
            string winTenTimes_UnlockCondition, string audio, string music, string sfx, string ui,
            string winFiveTimes_Name, string winFiveTimes_UnlockCondition, string unlimitedButton, string readyToggle,
            string randomizeButton, string battleResults_YouWon, string battleResults_YouLost,
            string battleResults_Restart, string battleResults_MainMenuButton)
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
            Sfx = sfx;
            UI = ui;
            WinFiveTimes_Name = winFiveTimes_Name;
            WinFiveTimes_UnlockCondition = winFiveTimes_UnlockCondition;
            UnlimitedButton = unlimitedButton;
            ReadyToggle = readyToggle;
            RandomizeButton = randomizeButton;
            BattleResults_YouWon = battleResults_YouWon;
            BattleResults_YouLost = battleResults_YouLost;
            BattleResults_Restart = battleResults_Restart;
            BattleResults_MainMenuButton = battleResults_MainMenuButton;
        }
    }
}