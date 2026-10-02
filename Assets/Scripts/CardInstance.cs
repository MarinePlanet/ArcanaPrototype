using System;
using UnityEngine;

namespace ArcanaPrototype
{
    [Serializable]
    public class CardInstance
    {
        [SerializeField] private CardData data;
        [SerializeField] private bool isTemporary;

        public CardData Data => data;
        public bool IsTemporary => isTemporary;

        public CardInstance(CardData cardData, bool temporary)
        {
            data = cardData;
            isTemporary = temporary;
        }
    }
}
