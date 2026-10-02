using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace ArcanaPrototype
{
    public enum OrientationDebugMode
    {
        UseCardProbabilities,
        ForceUpright,
        ForceReversed
    }

    [DisallowMultipleComponent]
    public class ArcanaResolver : MonoBehaviour
    {
        [Header("Scene References")]
        [SerializeField] private RectTransform playArea;
        [SerializeField] private Text resultText;

        [Header("Timing")]
        [SerializeField, Min(0.1f)] private float displayDuration = 2f;

        [Header("Battle References")]
        [SerializeField] private HandManager handManager;
        [SerializeField] private DeckManager deckManager;
        [SerializeField] private PlayerResource playerResource;
        [SerializeField] private PlayerHealth playerHealth;
        [SerializeField] private EnemyHealth enemyHealth;
        [SerializeField] private EnvironmentManager environmentManager;
        [SerializeField] private StatusManager playerStatusManager;
        [SerializeField] private StatusManager enemyStatusManager;

        [Header("Debug")]
        [SerializeField] private OrientationDebugMode orientationDebugMode;

        private bool isResolving;

        public bool IsResolving => isResolving;

        private void Start()
        {
            if (resultText != null)
            {
                resultText.text = "Play an Arcana card";
            }
        }

        public bool TryPlayCard(CardView card)
        {
            CardInstance cardInstance = card != null ? card.CardInstance : null;
            CardData cardData = cardInstance != null ? cardInstance.Data : null;

            if (isResolving || cardData == null || playArea == null || playerResource == null)
            {
                return false;
            }

            if (!playerResource.TrySpend(cardData.Cost))
            {
                if (resultText != null)
                {
                    resultText.text = "Not enough Resource for " + cardData.CardName;
                }

                return false;
            }

            if (handManager == null || !handManager.RemoveCardForPlay(cardInstance))
            {
                playerResource.Refund(cardData.Cost);
                return false;
            }

            isResolving = true;
            StartCoroutine(ResolveCard(card, cardInstance));
            return true;
        }

        public void ResetResolver()
        {
            StopAllCoroutines();
            isResolving = false;

            if (playArea != null)
            {
                CardView[] cardsInPlay = playArea.GetComponentsInChildren<CardView>(true);
                for (int i = 0; i < cardsInPlay.Length; i++)
                {
                    Destroy(cardsInPlay[i].gameObject);
                }
            }

            if (resultText != null)
            {
                resultText.text = "Play an Arcana card";
            }
        }

        private IEnumerator ResolveCard(CardView card, CardInstance cardInstance)
        {
            card.SetInteractionEnabled(false);

            RectTransform cardRect = card.GetComponent<RectTransform>();
            cardRect.SetParent(playArea, false);
            cardRect.anchorMin = new Vector2(0.5f, 0.5f);
            cardRect.anchorMax = new Vector2(0.5f, 0.5f);
            cardRect.pivot = new Vector2(0.5f, 0.5f);
            cardRect.anchoredPosition = Vector2.zero;

            ArcanaOrientation orientation = DetermineOrientation(cardInstance.Data);
            bool isReversed = orientation == ArcanaOrientation.Reversed;
            card.ShowOrientation(isReversed);

            if (resultText != null)
            {
                string description = orientation == ArcanaOrientation.Upright
                    ? cardInstance.Data.UprightDescription
                    : cardInstance.Data.ReversedDescription;
                resultText.text = card.CardName + "\n" + orientation.ToString().ToUpperInvariant() + "\n" + description;
            }

            CardEffectContext context = new CardEffectContext(
                cardInstance,
                orientation,
                handManager,
                deckManager,
                playerHealth,
                enemyHealth,
                environmentManager,
                playerStatusManager,
                enemyStatusManager);

            System.Collections.Generic.IReadOnlyList<CardEffect> effects = cardInstance.Data.GetEffects(orientation);
            for (int i = 0; i < effects.Count; i++)
            {
                if (effects[i] != null)
                {
                    yield return effects[i].Resolve(context);
                }
            }

            yield return new WaitForSeconds(displayDuration);

            MoveCardToDestination(cardInstance, context.Destination);
            Destroy(card.gameObject);
            if (resultText != null)
            {
                resultText.text = "Play an Arcana card";
            }

            isResolving = false;
        }

        private ArcanaOrientation DetermineOrientation(CardData cardData)
        {
            if (orientationDebugMode == OrientationDebugMode.ForceUpright)
            {
                return ArcanaOrientation.Upright;
            }

            if (orientationDebugMode == OrientationDebugMode.ForceReversed)
            {
                return ArcanaOrientation.Reversed;
            }

            float uprightWeight = Mathf.Max(0f, cardData.UprightProbability);
            float reversedWeight = Mathf.Max(0f, cardData.ReversedProbability);
            float totalWeight = uprightWeight + reversedWeight;

            if (totalWeight <= 0f)
            {
                return ArcanaOrientation.Upright;
            }

            return Random.value < uprightWeight / totalWeight
                ? ArcanaOrientation.Upright
                : ArcanaOrientation.Reversed;
        }

        private void MoveCardToDestination(CardInstance card, CardDestination destination)
        {
            if (deckManager == null || card == null)
            {
                return;
            }

            switch (destination)
            {
                case CardDestination.Exhaust:
                    deckManager.Exhaust(card);
                    break;
                case CardDestination.ReturnToHand:
                    if (handManager == null || !handManager.TryAddExistingCard(card))
                    {
                        deckManager.Discard(card);
                    }
                    break;
                case CardDestination.ShuffleIntoDrawPile:
                    deckManager.ShuffleIntoDrawPile(card);
                    break;
                default:
                    deckManager.Discard(card);
                    break;
            }
        }
    }
}
