using UnityEngine;

namespace ArcanaPrototype
{
    [CreateAssetMenu(fileName = "NewEnvironment", menuName = "Arcana/Environment Data")]
    public class EnvironmentData : ScriptableObject
    {
        [SerializeField] private string environmentName = "New Environment";
        [SerializeField, TextArea(2, 5)] private string description;

        public string EnvironmentName => environmentName;
        public string Description => description;
    }
}
