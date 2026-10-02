using System.Collections;
using UnityEngine;

namespace ArcanaPrototype
{
    public abstract class CardEffect : ScriptableObject
    {
        public abstract IEnumerator Resolve(CardEffectContext context);
    }
}
