using Data;
using EventBus;

namespace Events
{
    public struct OnPlayerStatisticsChanged : IEvent
    {
        public readonly PlayerStatistics PlayerStatistics;

        public OnPlayerStatisticsChanged(PlayerStatistics playerStatistics)
        {
            PlayerStatistics = playerStatistics;
        }
    }
}