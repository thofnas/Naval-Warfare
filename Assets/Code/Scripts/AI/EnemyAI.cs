using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using EventBus;
using Events;
using Grid;
using UnityEngine;
using Zenject;

namespace AI
{
    public class EnemyAI : MonoBehaviour
    {
        private IDifficulty _difficulty;
        private InteractionSystem _interactionSystem;
        private EventBinding<OnTurnChanged> _onTurnChangedBinding;

        private void OnEnable()
        {
            _onTurnChangedBinding = new EventBinding<OnTurnChanged>(TurnSystem_OnTurnChanged);
            EventBus<OnTurnChanged>.Register(_onTurnChangedBinding);
        }

        private void OnDisable() => EventBus<OnTurnChanged>.Deregister(_onTurnChangedBinding);

        [Inject]
        private void Construct(InteractionSystem interactionSystem, IDifficulty difficulty)
        {
            _interactionSystem = interactionSystem;
            SetDifficulty(difficulty);
        }

        private void SetDifficulty(IDifficulty difficultyStrategy) => _difficulty = difficultyStrategy;

        private void TakeEnemyAIAction()
        {
            _interactionSystem.SetSelectedCell(GetBestShootCellPosition());
            _interactionSystem.Shoot();
        }

        private CellPosition GetBestShootCellPosition()
        {
            List<EnemyAIAction> actions = _difficulty.CalculateActions();

            EnemyAIAction bestAction = actions.OrderByDescending(action => action.Points).First();

            return bestAction.CellPosition;
        }

        private void TurnSystem_OnTurnChanged(OnTurnChanged e)
        {
            if (e.CharacterType == CharacterType.Player)
                return;

            const float timer = 0.5f;

            DOVirtual.DelayedCall(timer, TakeEnemyAIAction);
        }
    }
}