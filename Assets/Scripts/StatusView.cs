using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace ArcanaPrototype
{
    [DisallowMultipleComponent]
    public class StatusView : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] private Text statusLabel;

        private StatusInstance status;
        private StatusTooltip tooltip;
        private RectTransform rectTransform;

        private void Awake()
        {
            rectTransform = GetComponent<RectTransform>();
        }

        public void Initialize(StatusInstance statusInstance, StatusTooltip statusTooltip)
        {
            status = statusInstance;
            tooltip = statusTooltip;

            if (statusLabel != null && status != null && status.Data != null)
            {
                statusLabel.text = status.Data.StatusName + " (" + status.RemainingTurns + ")";
            }
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            tooltip?.Show(status, rectTransform);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            tooltip?.Hide();
        }

        private void OnDisable()
        {
            tooltip?.Hide();
        }
    }
}
