using UnityEngine;

namespace ArcanaPrototype
{
    public abstract class StatusExpirationEffect : ScriptableObject
    {
        public abstract void Resolve(StatusEffectContext context);
    }

    public class StatusEffectContext
    {
        public EffectTarget Owner { get; }
        public StatusInstance Status { get; }
        public PlayerHealth PlayerHealth { get; }
        public EnemyHealth EnemyHealth { get; }

        public StatusEffectContext(
            EffectTarget owner,
            StatusInstance status,
            PlayerHealth playerHealth,
            EnemyHealth enemyHealth)
        {
            Owner = owner;
            Status = status;
            PlayerHealth = playerHealth;
            EnemyHealth = enemyHealth;
        }

        public void HealOwner(int amount)
        {
            if (Owner == EffectTarget.Player)
            {
                PlayerHealth?.Heal(amount);
            }
            else
            {
                EnemyHealth?.Heal(amount);
            }
        }
    }
}
