using Misc;
using Newtonsoft.Json;

namespace Data
{
    public class TextData
    {
        public readonly NonEmptyString Title;
        public readonly NonEmptyString PlayButton;
        public readonly NonEmptyString StoreButton;
        public readonly NonEmptyString SettingsButton;
        public readonly NonEmptyString AchievementsButton;
        public readonly NonEmptyString BackButton;
        public readonly NonEmptyString FramesPerSecond;
        public readonly NonEmptyString Language;
        public readonly NonEmptyString FirstPurchase;
        public readonly NonEmptyString BuyYourFirstMap;
        public readonly NonEmptyString Unlocked;


        [JsonConstructor]
        public TextData(NonEmptyString title, NonEmptyString playButton, NonEmptyString storeButton, NonEmptyString settingsButton, NonEmptyString achievementsButton, NonEmptyString backButton, NonEmptyString framesPerSecond, NonEmptyString language, NonEmptyString firstPurchase, NonEmptyString buyYourFirstMap, NonEmptyString unlocked)
        {
            Title = title;
            PlayButton = playButton;
            StoreButton = storeButton;
            SettingsButton = settingsButton;
            AchievementsButton = achievementsButton;
            BackButton = backButton;
            FramesPerSecond = framesPerSecond;
            Language = language;
            FirstPurchase = firstPurchase;
            BuyYourFirstMap = buyYourFirstMap;
            Unlocked = unlocked;
        }
    }
}