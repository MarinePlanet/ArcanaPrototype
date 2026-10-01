using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace ArcanaPrototype
{
    [DisallowMultipleComponent]
    public class ArcanaResolver : MonoBehaviour
    {
        [Header("Scene References")]
        [SerializeField] private RectTransform playArea;
        [SerializeField] private Text resultText;

        [Header("Timing")]
        [SerializeField, Min(0.1f)] private float displayDuration = 2f;

        private bool isResolving;

        private void Start()
        {
            if (resultText != null)
            {
                resultText.text = "Play an Arcana card";
            }
        }

        public bool TryPlayCard(CardView card)
        {
            if (isResolving || card == null || playArea == null)
            {
                return false;
            }

            StartCoroutine(ResolveCard(card));
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

        private IEnumerator ResolveCard(CardView card)
        {
            isResolving = true;
            card.SetInteractionEnabled(false);

            RectTransform cardRect = card.GetComponent<RectTransform>();
            cardRect.SetParent(playArea, false);
            cardRect.anchorMin = new Vector2(0.5f, 0.5f);
            cardRect.anchorMax = new Vector2(0.5f, 0.5f);
            cardRect.pivot = new Vector2(0.5f, 0.5f);
            cardRect.anchoredPosition = Vector2.zero;

            bool isReversed = Random.value < 0.5f;
            card.ShowOrientation(isReversed);

            if (resultText != null)
            {
                resultText.text = isReversed
                    ? card.CardName + "\nREVERSED\nStronger Effect + Drawback"
                    : card.CardName + "\nUPRIGHT\nNormal Effect";
            }

            yield return new WaitForSeconds(displayDuration);

            Destroy(card.gameObject);
            if (resultText != null)
            {
                resultText.text = "Play an Arcana card";
            }

            isResolving = false;
        }
    }
}
