using System.Collections;
using UnityEngine;

namespace ArcanaPrototype
{
    [CreateAssetMenu(fileName = "DrawCards", menuName = "Arcana/Card Effects/Draw Cards")]
    public class DrawCardsEffect : CardEffect
    {
        [SerializeField, Min(0)] private int amount = 1;

        public override IEnumerator Resolve(CardEffectContext context)
        {
            context?.HandManager?.DrawCards(amount);
            yield break;
        }
    }
}
