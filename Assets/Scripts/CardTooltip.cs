using UnityEngine;
using UnityEngine.UI;

namespace ArcanaPrototype
{
    [DisallowMultipleComponent]
    public class CardTooltip : MonoBehaviour
    {
        [SerializeField] private RectTransform tooltipRect;
        [SerializeField] private Text informationText;
        [SerializeField] private Vector2 screenOffset = new Vector2(0f, 150f);

        public void Show(CardData cardData, RectTransform cardRect)
        {
            if (cardData == null || cardRect == null)
            {
                return;
            }

            if (informationText != null)
            {
                informationText.text =
                    cardData.CardName + "\n\n" +
                    "Upright — " + FormatProbability(cardData.UprightProbability) + "\n" +
                    cardData.UprightDescription + "\n\n" +
                    "Reversed — " + FormatProbability(cardData.ReversedProbability) + "\n" +
                    cardData.ReversedDescription;
            }

            RectTransform target = tooltipRect != null ? tooltipRect : transform as RectTransform;
            if (target != null)
            {
                target.position = cardRect.position + (Vector3)screenOffset;
            }

            gameObject.SetActive(true);
            transform.SetAsLastSibling();
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }

        private static string FormatProbability(float probability)
        {
            return Mathf.RoundToInt(Mathf.Clamp01(probability) * 100f) + "%";
        }
    }
}
