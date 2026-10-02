using System.Collections;
using UnityEngine;

namespace ArcanaPrototype
{
    [CreateAssetMenu(fileName = "RandomEnvironment", menuName = "Arcana/Card Effects/Random Environment")]
    public class RandomEnvironmentEffect : CardEffect
    {
        public override IEnumerator Resolve(CardEffectContext context)
        {
            context?.EnvironmentManager?.ChangeToRandomDifferentEnvironment();
            yield break;
        }
    }
}
