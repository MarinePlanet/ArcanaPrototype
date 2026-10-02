using System.Collections;
using UnityEngine;

namespace ArcanaPrototype
{
    [CreateAssetMenu(fileName = "DamagePlayer", menuName = "Arcana/Card Effects/Damage Player")]
    public class DamagePlayerEffect : CardEffect
    {
        [SerializeField, Min(0)] private int amount = 1;

        public override IEnumerator Resolve(CardEffectContext context)
        {
            context?.PlayerHealth?.TakeDamage(amount);
            yield break;
        }
    }
}
