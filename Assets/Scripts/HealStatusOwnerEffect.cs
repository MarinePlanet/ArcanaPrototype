using UnityEngine;

namespace ArcanaPrototype
{
    [CreateAssetMenu(fileName = "HealStatusOwner", menuName = "Arcana/Status Effects/Heal Status Owner")]
    public class HealStatusOwnerEffect : StatusExpirationEffect
    {
        [SerializeField, Min(0)] private int amount = 1;

        public override void Resolve(StatusEffectContext context)
        {
            context?.HealOwner(amount);
        }
    }
}
