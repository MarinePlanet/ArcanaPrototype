using UnityEngine;
using UnityEngine.UI;

namespace ArcanaPrototype
{
    [DisallowMultipleComponent]
    public class PlayerResource : MonoBehaviour
    {
        [SerializeField, Min(0)] private int maximumResource = 4;
        [SerializeField] private Text resourceText;

        [Header("Runtime Debug")]
        [SerializeField] private int currentResource;

        public int CurrentResource => currentResource;
        public int MaximumResource => maximumResource;

        private void Awake()
        {
            ResetResource();
        }

        public bool CanAfford(int cost)
        {
            return currentResource >= Mathf.Max(0, cost);
        }

        public bool TrySpend(int cost)
        {
            int safeCost = Mathf.Max(0, cost);
            if (!CanAfford(safeCost))
            {
                return false;
            }

            currentResource -= safeCost;
            RefreshDisplay();
            return true;
        }

        public void Refund(int amount)
        {
            currentResource = Mathf.Clamp(currentResource + Mathf.Max(0, amount), 0, maximumResource);
            RefreshDisplay();
        }

        public void ResetResource()
        {
            currentResource = maximumResource;
            RefreshDisplay();
        }

        private void RefreshDisplay()
        {
            if (resourceText != null)
            {
                resourceText.text = "Resource: " + currentResource + " / " + maximumResource;
            }
        }
    }
}
