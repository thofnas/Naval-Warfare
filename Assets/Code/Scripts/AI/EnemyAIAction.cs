using Grid;

namespace AI
{
    public class EnemyAIAction
    {
        public readonly CellPosition CellPosition;
        public readonly int Points;

        public EnemyAIAction(CellPosition cellPosition, int points)
        {
            CellPosition = cellPosition;
            Points = points;
        }
    }
}