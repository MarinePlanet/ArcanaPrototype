using UnityEngine;

namespace ArcanaPrototype
{
    [DisallowMultipleComponent]
    public class BattleTurnManager : MonoBehaviour
    {
        [SerializeField, Min(1)] private int cardsDrawnPerTurn = 5;
        [SerializeField] private HandManager handManager;
        [SerializeField] private PlayerResource playerResource;
        [SerializeField] private StatusManager playerStatusManager;
        [SerializeField] private StatusManager enemyStatusManager;
        [SerializeField] private EnemyActionPool enemyActionPool;
        [SerializeField] private ArcanaResolver arcanaResolver;
        [SerializeField] private PlayerHealth playerHealth;
        [SerializeField] private EnemyHealth enemyHealth;

        private int playerTurnNumber;

        public int PlayerTurnNumber => playerTurnNumber;

        public void StartBattle()
        {
            playerTurnNumber = 1;
            playerResource?.ResetResource();
            handManager?.DrawCards(cardsDrawnPerTurn);
        }

        public void EndTurn()
        {
            if (arcanaResolver != null && arcanaResolver.IsResolving)
            {
                return;
            }

            handManager?.DiscardEntireHand();
            enemyActionPool?.TakeTurn();
            StartNextPlayerTurn();
        }

        private void StartNextPlayerTurn()
        {
            playerTurnNumber++;
            playerStatusManager?.ProcessPlayerTurnStart();
            enemyStatusManager?.ProcessPlayerTurnStart();

            if ((playerHealth != null && playerHealth.CurrentHealth <= 0) ||
                (enemyHealth != null && enemyHealth.CurrentHealth <= 0))
            {
                return;
            }

            playerResource?.ResetResource();
            handManager?.DrawCards(cardsDrawnPerTurn);
        }
    }
}
