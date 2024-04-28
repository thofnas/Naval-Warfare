using EventBus;
using Events;
using Newtonsoft.Json;

namespace Data
{
    public class PlayerStatistics
    {
        public int TotalBattles { get; private set; }
        public int Wins { get; private set; }
        public int Loses { get; private set; }
        public int Shots { get; private set; }
        public int Hits { get; private set; }

        [JsonIgnore] private EventBinding<OnCellHit> _onCellHit;
        [JsonIgnore] private EventBinding<OnBattleEnded> _onBattleEnded;

        public PlayerStatistics()
        {
            RegisterEvents();
        }

        [JsonConstructor]
        public PlayerStatistics(int totalBattles, int wins, int loses, int shots, int hits)
        {
            (TotalBattles, Wins, Loses, Shots, Hits) = (totalBattles, wins, loses, shots, hits);
            
            RegisterEvents();
        }

        private void RegisterEvents()
        {
            _onBattleEnded = new EventBinding<OnBattleEnded>(OnGameplayStateChanged);
            EventBus<OnBattleEnded>.Register(_onBattleEnded);
            _onCellHit = new EventBinding<OnCellHit>(OnCellHit);
            EventBus<OnCellHit>.Register(_onCellHit);
        }

        private void OnGameplayStateChanged(OnBattleEnded e)
        {
            if (e.IsWin) Wins++;
            else Loses++;

            TotalBattles++;
        }

        private void OnCellHit(OnCellHit e)
        {
            if (e.WoundedCharacterType == CharacterType.Player) return;

            if (e.Ship is not null)
                Hits++;

            Shots++;
        }
    }
}