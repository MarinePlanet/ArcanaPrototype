using UnityEngine;

namespace ArcanaPrototype
{
    [DisallowMultipleComponent]
    public class DeckViewController : MonoBehaviour
    {
        [SerializeField] private DeckData playerDeck;
        [SerializeField] private CardView cardPrefab;
        [SerializeField] private Transform cardContainer;
        [SerializeField] private CardTooltip cardTooltip;

        private void OnEnable()
        {
            RefreshDeckView();
        }

        public void RefreshDeckView()
        {
            ClearDeckView();

            if (playerDeck == null || cardPrefab == null || cardContainer == null)
            {
                return;
            }

            for (int i = 0; i < playerDeck.Cards.Count; i++)
            {
                CardData data = playerDeck.Cards[i];
                if (data == null)
                {
                    continue;
                }

                CardView view = Instantiate(cardPrefab, cardContainer);
                view.Initialize(new CardInstance(data, false), null, cardTooltip);
            }
        }

        private void ClearDeckView()
        {
            if (cardContainer == null)
            {
                return;
            }

            for (int i = cardContainer.childCount - 1; i >= 0; i--)
            {
                Destroy(cardContainer.GetChild(i).gameObject);
            }
        }
    }
}
