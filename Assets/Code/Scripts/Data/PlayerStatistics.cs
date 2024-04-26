using Newtonsoft.Json;

namespace Data
{
    public class PlayerStatistics
    {
        public int TotalBattles { get; } = 0;
        public int Wins { get; } = 0;
        public int Loses { get; } = 0;
        public int Shots { get; } = 0;
        public int Hits { get; } = 0;

        public PlayerStatistics()
        {
        }
        
        [JsonConstructor]
        public PlayerStatistics(int totalBattles, int wins, int loses, int shots, int hits)
        {
            TotalBattles = totalBattles;
            Wins = wins;
            Loses = loses;
            Shots = shots;
            Hits = hits;
        }
    }
}