using System.Collections.Generic;
using UnityEngine;

namespace ArcanaPrototype
{
    [DisallowMultipleComponent]
    public class DeckManager : MonoBehaviour
    {
        [SerializeField] private DeckData playerDeck;

        [Header("Runtime Debug")]
        [SerializeField] private List<CardInstance> drawPile = new List<CardInstance>();
        [SerializeField] private List<CardInstance> discardPile = new List<CardInstance>();
        [SerializeField] private List<CardInstance> exhaustPile = new List<CardInstance>();

        public DeckData PlayerDeck => playerDeck;
        public int DrawPileCount => drawPile.Count;
        public int DiscardPileCount => discardPile.Count;
        public int ExhaustPileCount => exhaustPile.Count;

        public void InitializeBattleDeck()
        {
            ClearBattleState();

            if (playerDeck == null)
            {
                Debug.LogWarning("DeckManager needs a Player Deck asset.", this);
                return;
            }

            IReadOnlyList<CardData> cards = playerDeck.Cards;
            for (int i = 0; i < cards.Count; i++)
            {
                if (cards[i] != null)
                {
                    drawPile.Add(new CardInstance(cards[i], false));
                }
            }

            Shuffle(drawPile);
        }

        public bool TryDraw(out CardInstance card)
        {
            if (drawPile.Count == 0)
            {
                ReshuffleDiscardIntoDrawPile();
            }

            if (drawPile.Count == 0)
            {
                card = null;
                return false;
            }

            int lastIndex = drawPile.Count - 1;
            card = drawPile[lastIndex];
            drawPile.RemoveAt(lastIndex);
            return true;
        }

        public void Discard(CardInstance card)
        {
            if (card != null)
            {
                discardPile.Add(card);
            }
        }

        public void Exhaust(CardInstance card)
        {
            if (card != null)
            {
                exhaustPile.Add(card);
            }
        }

        public void ShuffleIntoDrawPile(CardInstance card)
        {
            if (card == null)
            {
                return;
            }

            drawPile.Add(card);
            Shuffle(drawPile);
        }

        public void ClearBattleState()
        {
            drawPile.Clear();
            discardPile.Clear();
            exhaustPile.Clear();
        }

        [ContextMenu("Debug: Log Pile Counts")]
        private void LogPileCounts()
        {
            Debug.Log(
                "Draw: " + DrawPileCount +
                ", Discard: " + DiscardPileCount +
                ", Exhaust: " + ExhaustPileCount,
                this);
        }

        private void ReshuffleDiscardIntoDrawPile()
        {
            if (discardPile.Count == 0)
            {
                return;
            }

            drawPile.AddRange(discardPile);
            discardPile.Clear();
            Shuffle(drawPile);
        }

        private static void Shuffle(List<CardInstance> cards)
        {
            for (int i = cards.Count - 1; i > 0; i--)
            {
                int randomIndex = Random.Range(0, i + 1);
                CardInstance temporary = cards[i];
                cards[i] = cards[randomIndex];
                cards[randomIndex] = temporary;
            }
        }
    }
}
