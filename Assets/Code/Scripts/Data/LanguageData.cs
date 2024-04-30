using System.Data;
using EventBus;
using Events;

namespace Data
{
    public class LanguageData
    {
        public TextData TextData { get; private set; }

        public LanguageData(LanguageProvider languageProvider)
        {
            SetLanguage(languageProvider);

            EventBinding<OnPlayerPrefsLanguageChanged> onLanguageChanged = new(_ => SetLanguage(languageProvider));
            EventBus<OnPlayerPrefsLanguageChanged>.Register(onLanguageChanged);
        }

        private void SetLanguage(LanguageProvider languageProvider)
        {
            if (!languageProvider.TryLoad(GameSettings.GetLanguage(), out TextData textData))
                GameSettings.ResetLanguage();

            TextData = textData;
            
            EventBus<OnLanguageLoaded>.Invoke(new OnLanguageLoaded());
        }
    }
}