using System.Collections;
using UnityEngine;

namespace ArcanaPrototype
{
    [CreateAssetMenu(fileName = "DamageEnemy", menuName = "Arcana/Card Effects/Damage Enemy")]
    public class DamageEnemyEffect : CardEffect
    {
        [SerializeField, Min(0)] private int amount = 1;

        public override IEnumerator Resolve(CardEffectContext context)
        {
            context?.EnemyHealth?.TakeDamage(amount);
            yield break;
        }
    }
}
