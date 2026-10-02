using System.Collections.Generic;
using UnityEngine;

namespace ArcanaPrototype
{
    [DisallowMultipleComponent]
    public class StatusManager : MonoBehaviour
    {
        [SerializeField] private EffectTarget owner;
        [SerializeField] private PlayerHealth playerHealth;
        [SerializeField] private EnemyHealth enemyHealth;

        [Header("Status UI")]
        [SerializeField] private StatusView statusViewPrefab;
        [SerializeField] private Transform statusContainer;
        [SerializeField] private StatusTooltip statusTooltip;

        [Header("Runtime Debug")]
        [SerializeField] private List<StatusInstance> activeStatuses = new List<StatusInstance>();

        public IReadOnlyList<StatusInstance> ActiveStatuses => activeStatuses;

        public void AddStatus(StatusData data, int duration, StatusSource source)
        {
            if (data == null)
            {
                return;
            }

            activeStatuses.Add(new StatusInstance(data, duration, source));
            RefreshStatusViews();
        }

        public void ProcessPlayerTurnStart()
        {
            for (int i = activeStatuses.Count - 1; i >= 0; i--)
            {
                StatusInstance status = activeStatuses[i];
                status.DecrementDuration();

                if (status.RemainingTurns > 0)
                {
                    continue;
                }

                StatusEffectContext context = new StatusEffectContext(owner, status, playerHealth, enemyHealth);
                IReadOnlyList<StatusExpirationEffect> effects = status.Data.ExpirationEffects;
                for (int effectIndex = 0; effectIndex < effects.Count; effectIndex++)
                {
                    effects[effectIndex]?.Resolve(context);
                }

                activeStatuses.RemoveAt(i);
            }

            RefreshStatusViews();
        }

        public void ClearStatuses()
        {
            activeStatuses.Clear();
            RefreshStatusViews();
        }

        [ContextMenu("Debug: Log Active Statuses")]
        private void LogActiveStatuses()
        {
            if (activeStatuses.Count == 0)
            {
                Debug.Log(name + " has no active statuses.", this);
                return;
            }

            for (int i = 0; i < activeStatuses.Count; i++)
            {
                StatusInstance status = activeStatuses[i];
                Debug.Log(
                    status.Data.StatusName + " — " + status.RemainingTurns +
                    " turn(s), Source: " + status.Source.DisplayText,
                    this);
            }
        }

        private void RefreshStatusViews()
        {
            if (statusContainer == null)
            {
                return;
            }

            for (int i = statusContainer.childCount - 1; i >= 0; i--)
            {
                Destroy(statusContainer.GetChild(i).gameObject);
            }

            if (statusViewPrefab == null)
            {
                return;
            }

            for (int i = 0; i < activeStatuses.Count; i++)
            {
                StatusView view = Instantiate(statusViewPrefab, statusContainer);
                view.Initialize(activeStatuses[i], statusTooltip);
            }
        }
    }
}
