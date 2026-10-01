using UnityEngine;
using UnityEngine.UI;

namespace ArcanaPrototype
{
    [DisallowMultipleComponent]
    public class HandManager : MonoBehaviour
    {
        [Header("Scene References")]
        [SerializeField] private CardView cardPrefab;
        [SerializeField] private Transform handContainer;
        [SerializeField] private ArcanaResolver arcanaResolver;

        [Header("Prototype Settings")]
        [SerializeField, Min(1)] private int maximumHandSize = 8;
        [SerializeField] private Text handMessageText;

        private readonly string[] placeholderNames =
        {
            "The Fool",
            "The Magician",
            "The High Priestess",
            "The Empress",
            "The Emperor",
            "The Hermit",
            "The Moon",
            "The Sun"
        };

        private readonly Color[] placeholderColors =
        {
            new Color(0.91f, 0.76f, 0.38f),
            new Color(0.62f, 0.42f, 0.78f),
            new Color(0.35f, 0.58f, 0.78f),
            new Color(0.73f, 0.38f, 0.48f),
            new Color(0.42f, 0.67f, 0.52f)
        };

        public void DrawCard()
        {
            if (cardPrefab == null || handContainer == null)
            {
                Debug.LogWarning("HandManager needs a Card Prefab and Hand Container.", this);
                return;
            }

            if (handContainer.childCount >= maximumHandSize)
            {
                SetHandMessage("Hand is full.");
                return;
            }

            string cardName = placeholderNames[Random.Range(0, placeholderNames.Length)];
            Color cardColor = placeholderColors[Random.Range(0, placeholderColors.Length)];
            CardView card = Instantiate(cardPrefab, handContainer);
            card.Initialize(cardName, cardColor, this);
            SetHandMessage("Drew " + cardName);
        }

        public void TryPlayCard(CardView card)
        {
            if (card == null || arcanaResolver == null)
            {
                Debug.LogWarning("HandManager needs an Arcana Resolver.", this);
                return;
            }

            if (arcanaResolver.TryPlayCard(card))
            {
                SetHandMessage(string.Empty);
            }
        }

        public void ResetHand()
        {
            if (handContainer == null)
            {
                return;
            }

            for (int i = handContainer.childCount - 1; i >= 0; i--)
            {
                Destroy(handContainer.GetChild(i).gameObject);
            }

            SetHandMessage(string.Empty);
        }

        private void SetHandMessage(string message)
        {
            if (handMessageText != null)
            {
                handMessageText.text = message;
            }
        }
    }
}
