using System;
using EventBus;
using Events;
using JetBrains.Annotations;

namespace Data
{
    public class LanguageData
    {
        public TextData TextData { get; private set; }

        public LanguageData(LanguageProvider languageProvider)
        {
            SetTextData(languageProvider, () => EventBus<OnLanguageLoaded>.Invoke(new OnLanguageLoaded()));

            EventBinding<OnPlayerPrefsLanguageChanged> onLanguageChanged = new(_ => SetTextData(languageProvider, () => EventBus<OnLanguageLoaded>.Invoke(new OnLanguageLoaded())));
            EventBus<OnPlayerPrefsLanguageChanged>.Register(onLanguageChanged);
        }

        private void SetTextData(LanguageProvider languageProvider, [CanBeNull] Action onComplete)
        {
            TextData = GetTextData(languageProvider, GameSettings.GetLanguage());
            
            onComplete?.Invoke();
        }

        private TextData GetTextData(LanguageProvider languageProvider, string language)
        {
            languageProvider.TryLoad(language, out TextData textData);

            return textData;
        }
    }
}