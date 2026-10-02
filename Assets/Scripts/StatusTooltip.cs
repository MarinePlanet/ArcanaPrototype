using UnityEngine;
using UnityEngine.UI;

namespace ArcanaPrototype
{
    [DisallowMultipleComponent]
    public class StatusTooltip : MonoBehaviour
    {
        [SerializeField] private RectTransform tooltipRect;
        [SerializeField] private Text informationText;
        [SerializeField] private Vector2 screenOffset = new Vector2(0f, 100f);

        public void Show(StatusInstance status, RectTransform statusRect)
        {
            if (status == null || status.Data == null || statusRect == null)
            {
                return;
            }

            if (informationText != null)
            {
                string source = status.Source != null ? status.Source.DisplayText : "Unknown";
                informationText.text =
                    status.Data.StatusName + "\n\n" +
                    status.Data.Description + "\n\n" +
                    "Remaining: " + status.RemainingTurns + " turn(s)\n" +
                    "Source: " + source;
            }

            RectTransform target = tooltipRect != null ? tooltipRect : transform as RectTransform;
            if (target != null)
            {
                target.position = statusRect.position + (Vector3)screenOffset;
            }

            gameObject.SetActive(true);
            transform.SetAsLastSibling();
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }
    }
}
