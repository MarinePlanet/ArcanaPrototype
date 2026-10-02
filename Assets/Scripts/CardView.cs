using System;
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
        [SerializeField] private Text costText;
        [SerializeField] private Text typeText;
        [SerializeField] private Image backgroundImage;

        [Header("Hover")]
        [SerializeField, Min(1f)] private float hoverScale = 1.1f;

        private Action<CardView> clickHandler;
        private CardTooltip tooltip;
        private RectTransform rectTransform;
        private Vector3 normalScale = Vector3.one;
        private bool canInteract = true;

        public CardInstance CardInstance { get; private set; }
        public string CardName => CardInstance != null && CardInstance.Data != null
            ? CardInstance.Data.CardName
            : string.Empty;

        private void Awake()
        {
            rectTransform = GetComponent<RectTransform>();
            normalScale = rectTransform.localScale;
        }

        public void Initialize(CardInstance cardInstance, Action<CardView> onClicked, CardTooltip cardTooltip)
        {
            CardInstance = cardInstance;
            clickHandler = onClicked;
            tooltip = cardTooltip;
            canInteract = true;

            CardData data = cardInstance != null ? cardInstance.Data : null;

            if (titleText != null)
            {
                titleText.text = data != null ? data.CardName : "Missing Card";
            }

            if (costText != null)
            {
                costText.text = data != null ? data.Cost.ToString() : "-";
            }

            if (typeText != null)
            {
                typeText.text = data != null ? data.CardType.ToString().ToUpperInvariant() : string.Empty;
            }

            if (backgroundImage != null)
            {
                backgroundImage.color = data != null ? data.CardColor : Color.gray;
            }

            rectTransform.localEulerAngles = Vector3.zero;
            rectTransform.localScale = normalScale;
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (canInteract)
            {
                rectTransform.localScale = normalScale * hoverScale;
                tooltip?.Show(CardInstance != null ? CardInstance.Data : null, rectTransform);
            }
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            rectTransform.localScale = normalScale;
            tooltip?.Hide();
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (canInteract && eventData.button == PointerEventData.InputButton.Left && clickHandler != null)
            {
                clickHandler(this);
            }
        }

        public void SetInteractionEnabled(bool enabled)
        {
            canInteract = enabled;
            rectTransform.localScale = normalScale;
            if (!enabled)
            {
                tooltip?.Hide();
            }
        }

        public void ShowOrientation(bool isReversed)
        {
            rectTransform.localEulerAngles = isReversed
                ? new Vector3(0f, 0f, 180f)
                : Vector3.zero;
        }

        private void OnDisable()
        {
            tooltip?.Hide();
        }
    }
}
