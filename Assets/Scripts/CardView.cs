using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace ArcanaPrototype
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(RectTransform))]
    public class CardView : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
    {
        [Header("Card UI")]
        [SerializeField] private Text titleText;
        [SerializeField] private Image backgroundImage;

        [Header("Hover")]
        [SerializeField, Min(1f)] private float hoverScale = 1.1f;

        private HandManager handManager;
        private RectTransform rectTransform;
        private Vector3 normalScale = Vector3.one;
        private bool canInteract = true;

        public string CardName { get; private set; }

        private void Awake()
        {
            rectTransform = GetComponent<RectTransform>();
            normalScale = rectTransform.localScale;
        }

        public void Initialize(string cardName, Color cardColor, HandManager owner)
        {
            CardName = cardName;
            handManager = owner;
            canInteract = true;

            if (titleText != null)
            {
                titleText.text = cardName;
            }

            if (backgroundImage != null)
            {
                backgroundImage.color = cardColor;
            }

            rectTransform.localEulerAngles = Vector3.zero;
            rectTransform.localScale = normalScale;
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (canInteract)
            {
                rectTransform.localScale = normalScale * hoverScale;
            }
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            rectTransform.localScale = normalScale;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (canInteract && eventData.button == PointerEventData.InputButton.Left && handManager != null)
            {
                handManager.TryPlayCard(this);
            }
        }

        public void SetInteractionEnabled(bool enabled)
        {
            canInteract = enabled;
            rectTransform.localScale = normalScale;
        }

        public void ShowOrientation(bool isReversed)
        {
            rectTransform.localEulerAngles = isReversed
                ? new Vector3(0f, 0f, 180f)
                : Vector3.zero;
        }
    }
}
