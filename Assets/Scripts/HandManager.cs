using System.Collections.Generic;
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
        [SerializeField] private DeckManager deckManager;
        [SerializeField] private CardTooltip cardTooltip;

        [Header("Prototype Settings")]
        [SerializeField, Min(1)] private int maximumHandSize = 8;
        [SerializeField] private Text handMessageText;
        [SerializeField] private Text handCountText;

        [Header("Runtime Debug")]
        [SerializeField] private List<CardInstance> cardsInHand = new List<CardInstance>();

        private readonly Dictionary<CardInstance, CardView> cardViews = new Dictionary<CardInstance, CardView>();

        public int HandCount => cardsInHand.Count;
        public int MaximumHandSize => maximumHandSize;

        public void DrawCard()
        {
            DrawCards(1);
        }

        public int DrawCards(int requestedCount)
        {
            if (deckManager == null)
            {
                Debug.LogWarning("HandManager needs a DeckManager.", this);
                return 0;
            }

            if (cardPrefab == null || handContainer == null)
            {
                Debug.LogWarning("HandManager needs a Card Prefab and Hand Container.", this);
                return 0;
            }

            int availableSlots = Mathf.Max(0, maximumHandSize - cardsInHand.Count);
            int drawsToAttempt = Mathf.Min(Mathf.Max(0, requestedCount), availableSlots);
            int cardsDrawn = 0;

            for (int i = 0; i < drawsToAttempt; i++)
            {
                if (!deckManager.TryDraw(out CardInstance card))
                {
                    break;
                }

                AddCardToHand(card);
                cardsDrawn++;
            }

            SetHandMessage(cardsDrawn > 0 ? "Drew " + cardsDrawn + " card(s)." : string.Empty);
            UpdateHandCount();
            return cardsDrawn;
        }

        public bool TryAddCreatedCard(CardData cardData, bool isTemporary = true)
        {
            if (cardData == null || cardsInHand.Count >= maximumHandSize)
            {
                return false;
            }

            if (!AddCardToHand(new CardInstance(cardData, isTemporary)))
            {
                return false;
            }

            UpdateHandCount();
            return true;
        }

        public bool TryAddExistingCard(CardInstance card)
        {
            if (card == null || cardsInHand.Count >= maximumHandSize)
            {
                return false;
            }

            if (!AddCardToHand(card))
            {
                return false;
            }

            UpdateHandCount();
            return true;
        }

        public void TryPlayCard(CardView card)
        {
            if (card == null || arcanaResolver == null)
            {
                return;
            }

            arcanaResolver.TryPlayCard(card);
        }

        public bool RemoveCardForPlay(CardInstance card)
        {
            if (card == null || !cardsInHand.Remove(card))
            {
                return false;
            }

            cardViews.Remove(card);
            SetHandMessage(string.Empty);
            UpdateHandCount();
            return true;
        }

        public int DiscardRandomCards(int requestedCount)
        {
            int discardCount = Mathf.Min(Mathf.Max(0, requestedCount), cardsInHand.Count);

            for (int i = 0; i < discardCount; i++)
            {
                int index = Random.Range(0, cardsInHand.Count);
                CardInstance card = cardsInHand[index];
                cardsInHand.RemoveAt(index);

                if (cardViews.TryGetValue(card, out CardView view))
                {
                    cardViews.Remove(card);
                    Destroy(view.gameObject);
                }

                deckManager?.Discard(card);
            }

            UpdateHandCount();
            return discardCount;
        }

        public void DiscardEntireHand()
        {
            for (int i = 0; i < cardsInHand.Count; i++)
            {
                deckManager?.Discard(cardsInHand[i]);
            }

            cardsInHand.Clear();
            cardViews.Clear();
            DestroyAllHandViews();
            SetHandMessage(string.Empty);
            UpdateHandCount();
        }

        public void ResetHand()
        {
            cardsInHand.Clear();
            cardViews.Clear();
            DestroyAllHandViews();
            SetHandMessage(string.Empty);
            UpdateHandCount();
        }

        private bool AddCardToHand(CardInstance card)
        {
            if (card == null || cardPrefab == null || handContainer == null)
            {
                return false;
            }

            cardsInHand.Add(card);
            CardView view = Instantiate(cardPrefab, handContainer);
            view.Initialize(card, TryPlayCard, cardTooltip);
            cardViews[card] = view;
            return true;
        }

        private void DestroyAllHandViews()
        {
            if (handContainer == null)
            {
                return;
            }

            for (int i = handContainer.childCount - 1; i >= 0; i--)
            {
                Destroy(handContainer.GetChild(i).gameObject);
            }
        }

        private void SetHandMessage(string message)
        {
            if (handMessageText != null)
            {
                handMessageText.text = message;
            }
        }

        private void UpdateHandCount()
        {
            if (handCountText != null)
            {
                handCountText.text = "Hand: " + cardsInHand.Count + " / " + maximumHandSize;
            }
        }
    }
}
