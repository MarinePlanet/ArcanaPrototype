using System.Collections.Generic;
using UnityEngine;

namespace ArcanaPrototype
{
    [CreateAssetMenu(fileName = "PlayerDeck", menuName = "Arcana/Deck Data")]
    public class DeckData : ScriptableObject
    {
        [SerializeField] private List<CardData> cards = new List<CardData>();

        public IReadOnlyList<CardData> Cards => cards;
    }
}
