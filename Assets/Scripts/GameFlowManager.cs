using UnityEngine;

namespace ArcanaPrototype
{
    [DisallowMultipleComponent]
    public class GameFlowManager : MonoBehaviour
    {
        [Header("Screens")]
        [SerializeField] private GameObject mainMenuPanel;
        [SerializeField] private GameObject deckViewPanel;
        [SerializeField] private GameObject victoryPanel;
        [SerializeField] private GameObject defeatPanel;

        [Header("Existing Battle UI")]
        [Tooltip("Assign each existing top-level battle UI object. No reparenting is required.")]
        [SerializeField] private GameObject[] battleUIObjects;

        [Header("Required Battle References")]
        [SerializeField] private PlayerHealth playerHealth;
        [SerializeField] private EnemyHealth enemyHealth;

        [Header("Optional Battle Reset References")]
        [SerializeField] private HandManager handManager;
        [SerializeField] private ArcanaResolver arcanaResolver;
        [SerializeField] private EnemyActionPool enemyActionPool;
        [SerializeField] private EnvironmentManager environmentManager;
        [SerializeField] private DeckManager deckManager;
        [SerializeField] private PlayerResource playerResource;
        [SerializeField] private BattleTurnManager battleTurnManager;
        [SerializeField] private StatusManager playerStatusManager;
        [SerializeField] private StatusManager enemyStatusManager;

        private bool isBattleActive;

        private void Awake()
        {
            if (playerHealth != null)
            {
                playerHealth.HealthDepleted += HandlePlayerDefeated;
            }

            if (enemyHealth != null)
            {
                enemyHealth.HealthDepleted += HandleEnemyDefeated;
            }
        }

        private void Start()
        {
            ShowMainMenu();
        }

        private void OnDestroy()
        {
            if (playerHealth != null)
            {
                playerHealth.HealthDepleted -= HandlePlayerDefeated;
            }

            if (enemyHealth != null)
            {
                enemyHealth.HealthDepleted -= HandleEnemyDefeated;
            }
        }

        public void StartGame()
        {
            ShowOnlyBattleUI();
            ResetBattle();
            deckManager?.InitializeBattleDeck();
            isBattleActive = true;
            battleTurnManager?.StartBattle();
        }

        public void ShowDeckView()
        {
            isBattleActive = false;
            SetAllScreens(false);
            SetBattleUIActive(false);

            if (deckViewPanel != null)
            {
                deckViewPanel.SetActive(true);
            }
        }

        public void ShowMainMenu()
        {
            isBattleActive = false;
            ResetBattle();
            SetAllScreens(false);
            SetBattleUIActive(false);

            if (mainMenuPanel != null)
            {
                mainMenuPanel.SetActive(true);
            }
        }

        private void HandleEnemyDefeated()
        {
            if (!isBattleActive)
            {
                return;
            }

            isBattleActive = false;
            SetAllScreens(false);
            SetBattleUIActive(false);

            if (victoryPanel != null)
            {
                victoryPanel.SetActive(true);
            }
        }

        private void HandlePlayerDefeated()
        {
            if (!isBattleActive)
            {
                return;
            }

            isBattleActive = false;
            SetAllScreens(false);
            SetBattleUIActive(false);

            if (defeatPanel != null)
            {
                defeatPanel.SetActive(true);
            }
        }

        private void ShowOnlyBattleUI()
        {
            SetAllScreens(false);
            SetBattleUIActive(true);
        }

        private void SetAllScreens(bool active)
        {
            SetActiveIfAssigned(mainMenuPanel, active);
            SetActiveIfAssigned(deckViewPanel, active);
            SetActiveIfAssigned(victoryPanel, active);
            SetActiveIfAssigned(defeatPanel, active);
        }

        private void SetBattleUIActive(bool active)
        {
            if (battleUIObjects == null)
            {
                return;
            }

            for (int i = 0; i < battleUIObjects.Length; i++)
            {
                SetActiveIfAssigned(battleUIObjects[i], active);
            }
        }

        private void ResetBattle()
        {
            if (arcanaResolver != null)
            {
                arcanaResolver.ResetResolver();
            }

            if (handManager != null)
            {
                handManager.ResetHand();
            }

            deckManager?.ClearBattleState();
            playerStatusManager?.ClearStatuses();
            enemyStatusManager?.ClearStatuses();
            playerResource?.ResetResource();

            if (enemyActionPool != null)
            {
                enemyActionPool.ResetActionPool();
            }

            if (environmentManager != null)
            {
                environmentManager.ResetEnvironment();
            }

            if (playerHealth != null)
            {
                playerHealth.ResetToFullHealth();
            }

            if (enemyHealth != null)
            {
                enemyHealth.ResetToFullHealth();
            }
        }

        private static void SetActiveIfAssigned(GameObject target, bool active)
        {
            if (target != null)
            {
                target.SetActive(active);
            }
        }
    }
}
