using System.Collections.Generic;
using UnityEngine;

namespace ArcanaPrototype
{
    public enum CardType
    {
        Attack,
        Utility
    }

    public enum ArcanaOrientation
    {
        Upright,
        Reversed
    }

    [CreateAssetMenu(fileName = "NewCard", menuName = "Arcana/Card Data")]
    public class CardData : ScriptableObject
    {
        [Header("Identity")]
        [SerializeField] private string cardName = "New Arcana";
        [SerializeField] private CardType cardType;
        [SerializeField, Min(0)] private int cost = 1;
        [SerializeField] private Color cardColor = Color.white;

        [Header("Upright")]
        [SerializeField, Range(0f, 1f)] private float uprightProbability = 0.5f;
        [SerializeField, TextArea(2, 5)] private string uprightDescription;
        [SerializeField] private List<CardEffect> uprightEffects = new List<CardEffect>();

        [Header("Reversed")]
        [SerializeField, Range(0f, 1f)] private float reversedProbability = 0.5f;
        [SerializeField, TextArea(2, 5)] private string reversedDescription;
        [SerializeField] private List<CardEffect> reversedEffects = new List<CardEffect>();

        public string CardName => cardName;
        public CardType CardType => cardType;
        public int Cost => cost;
        public Color CardColor => cardColor;
        public float UprightProbability => uprightProbability;
        public string UprightDescription => uprightDescription;
        public float ReversedProbability => reversedProbability;
        public string ReversedDescription => reversedDescription;

        public IReadOnlyList<CardEffect> GetEffects(ArcanaOrientation orientation)
        {
            return orientation == ArcanaOrientation.Upright ? uprightEffects : reversedEffects;
        }
    }
}
