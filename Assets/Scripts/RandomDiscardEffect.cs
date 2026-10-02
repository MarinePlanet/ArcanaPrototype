using System.Collections;
using UnityEngine;

namespace ArcanaPrototype
{
    [CreateAssetMenu(fileName = "RandomDiscard", menuName = "Arcana/Card Effects/Random Discard")]
    public class RandomDiscardEffect : CardEffect
    {
        [SerializeField, Min(0)] private int amount = 1;

        public override IEnumerator Resolve(CardEffectContext context)
        {
            context?.HandManager?.DiscardRandomCards(amount);
            yield break;
        }
    }
}
