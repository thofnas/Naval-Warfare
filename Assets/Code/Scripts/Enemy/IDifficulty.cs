using System.Collections.Generic;

namespace Enemy
{
    public interface IDifficulty
    {
        List<EnemyAIAction> CalculateActions();
    }
}