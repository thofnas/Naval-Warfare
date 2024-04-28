using EventBus;
using Events;

namespace Data
{
    public class DataSaver
    {
        private readonly LocalDataProvider _localDataProvider;
    
        public DataSaver(LocalDataProvider localDataProvider)
        {
            _localDataProvider = localDataProvider;

            RegisterEvents();
        }

        private void RegisterEvents()
        {
            EventBus<OnBattleEnded>.Register(new EventBinding<OnBattleEnded>(SaveGame));
            EventBus<OnThemeChanged>.Register(new EventBinding<OnThemeChanged>(SaveGame));
        }

        private void SaveGame() => _localDataProvider.Save();
    }
}