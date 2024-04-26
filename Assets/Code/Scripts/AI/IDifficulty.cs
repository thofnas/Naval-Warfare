using System.Collections.Generic;

namespace AI
{
    public interface IDifficulty
    {
        List<EnemyAIAction> CalculateActions();

        public int GetWinMoneyAmount();
    }
}