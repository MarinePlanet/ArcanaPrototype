using System.Collections;
using UnityEngine;

namespace ArcanaPrototype
{
    [CreateAssetMenu(fileName = "ApplyStatus", menuName = "Arcana/Card Effects/Apply Status")]
    public class ApplyStatusEffect : CardEffect
    {
        [SerializeField] private StatusData status;
        [SerializeField] private EffectTarget target = EffectTarget.Enemy;
        [SerializeField, Min(1)] private int duration = 1;

        public override IEnumerator Resolve(CardEffectContext context)
        {
            if (context == null || context.SourceCard == null || context.SourceCard.Data == null || status == null)
            {
                yield break;
            }

            StatusManager manager = target == EffectTarget.Player
                ? context.PlayerStatusManager
                : context.EnemyStatusManager;

            if (manager != null)
            {
                string sourceName = context.SourceCard.Data.CardName + " (" + context.Orientation + ")";
                manager.AddStatus(
                    status,
                    duration,
                    new StatusSource(StatusSourceType.Card, sourceName));
            }

            yield break;
        }
    }
}
