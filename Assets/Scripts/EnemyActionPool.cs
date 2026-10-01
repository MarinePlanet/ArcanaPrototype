using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace ArcanaPrototype
{
    [Serializable]
    public class EnemyAction
    {
        public string actionName;
        [HideInInspector] public bool hasBeenUsed;
    }

    [DisallowMultipleComponent]
    public class EnemyActionPool : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [Header("Action Pool")]
        [SerializeField] private List<EnemyAction> actions = new List<EnemyAction>
        {
            new EnemyAction { actionName = "Attack" },
            new EnemyAction { actionName = "Guard" },
            new EnemyAction { actionName = "Debuff" },
            new EnemyAction { actionName = "Heavy Attack" }
        };

        [Header("Scene References")]
        [SerializeField] private GameObject actionPoolPanel;
        [SerializeField] private Text actionPoolText;
        [SerializeField] private Text selectedActionText;

        private void Start()
        {
            ResetActionPool();
        }

        public void ResetActionPool()
        {
            for (int i = 0; i < actions.Count; i++)
            {
                actions[i].hasBeenUsed = false;
            }

            RefreshActionPoolText();

            if (actionPoolPanel != null)
            {
                actionPoolPanel.SetActive(false);
            }

            if (selectedActionText != null)
            {
                selectedActionText.text = "Enemy is waiting...";
            }
        }

        public void TakeTurn()
        {
            if (actions.Count == 0)
            {
                SetSelectedActionText("Enemy has no configured actions.");
                return;
            }

            if (AllActionsUsed())
            {
                ResetActionPool();
            }

            List<int> availableIndices = new List<int>();
            for (int i = 0; i < actions.Count; i++)
            {
                if (!actions[i].hasBeenUsed)
                {
                    availableIndices.Add(i);
                }
            }

            int selectedIndex = availableIndices[UnityEngine.Random.Range(0, availableIndices.Count)];
            EnemyAction selectedAction = actions[selectedIndex];
            selectedAction.hasBeenUsed = true;
            SetSelectedActionText("Enemy selected: " + selectedAction.actionName);
            RefreshActionPoolText();
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            RefreshActionPoolText();
            if (actionPoolPanel != null)
            {
                actionPoolPanel.SetActive(true);
            }
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (actionPoolPanel != null)
            {
                actionPoolPanel.SetActive(false);
            }
        }

        private bool AllActionsUsed()
        {
            for (int i = 0; i < actions.Count; i++)
            {
                if (!actions[i].hasBeenUsed)
                {
                    return false;
                }
            }

            return true;
        }

        private void RefreshActionPoolText()
        {
            if (actionPoolText == null)
            {
                return;
            }

            StringBuilder builder = new StringBuilder("ACTION POOL\n\n");
            for (int i = 0; i < actions.Count; i++)
            {
                EnemyAction action = actions[i];
                builder.Append(action.actionName);
                builder.Append("    ");
                builder.AppendLine(action.hasBeenUsed ? "USED" : "AVAILABLE");
            }

            actionPoolText.text = builder.ToString();
        }

        private void SetSelectedActionText(string message)
        {
            if (selectedActionText != null)
            {
                selectedActionText.text = message;
            }
        }
    }
}
