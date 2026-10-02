using System;
using System.Collections.Generic;
using UnityEngine;

namespace ArcanaPrototype
{
    public enum EffectTarget
    {
        Player,
        Enemy
    }

    public enum StatusSourceType
    {
        Card,
        EnemyAction,
        Environment
    }

    [Serializable]
    public class StatusSource
    {
        [SerializeField] private StatusSourceType sourceType;
        [SerializeField] private string sourceName;

        public StatusSourceType SourceType => sourceType;
        public string SourceName => sourceName;
        public string DisplayText => sourceType + " — " + sourceName;

        public StatusSource(StatusSourceType type, string name)
        {
            sourceType = type;
            sourceName = name;
        }
    }

    [Serializable]
    public class StatusInstance
    {
        [SerializeField] private StatusData data;
        [SerializeField] private int remainingTurns;
        [SerializeField] private StatusSource source;

        public StatusData Data => data;
        public int RemainingTurns => remainingTurns;
        public StatusSource Source => source;

        public StatusInstance(StatusData statusData, int duration, StatusSource statusSource)
        {
            data = statusData;
            remainingTurns = Mathf.Max(1, duration);
            source = statusSource;
        }

        public void DecrementDuration()
        {
            remainingTurns = Mathf.Max(0, remainingTurns - 1);
        }
    }

    [CreateAssetMenu(fileName = "NewStatus", menuName = "Arcana/Status Data")]
    public class StatusData : ScriptableObject
    {
        [SerializeField] private string statusName = "New Status";
        [SerializeField, TextArea(2, 5)] private string description;
        [SerializeField] private List<StatusExpirationEffect> expirationEffects = new List<StatusExpirationEffect>();

        public string StatusName => statusName;
        public string Description => description;
        public IReadOnlyList<StatusExpirationEffect> ExpirationEffects => expirationEffects;
    }
}
