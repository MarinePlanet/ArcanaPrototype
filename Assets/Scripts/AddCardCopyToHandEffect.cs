using System.Collections;
using UnityEngine;

namespace ArcanaPrototype
{
    [CreateAssetMenu(fileName = "AddCardCopyToHand", menuName = "Arcana/Card Effects/Add Card Copy To Hand")]
    public class AddCardCopyToHandEffect : CardEffect
    {
        [SerializeField] private CardData cardToCopy;
        [SerializeField, Min(1)] private int numberOfCopies = 1;

        public override IEnumerator Resolve(CardEffectContext context)
        {
            if (context?.HandManager == null || cardToCopy == null)
            {
                yield break;
            }

            for (int i = 0; i < numberOfCopies; i++)
            {
                if (!context.HandManager.TryAddCreatedCard(cardToCopy, true))
                {
                    break;
                }
            }

            yield break;
        }
    }
}
