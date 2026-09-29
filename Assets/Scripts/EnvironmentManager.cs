using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ArcanaPrototype
{
    [Serializable]
    public class EnvironmentCondition
    {
        public string environmentName;
        [TextArea] public string description;
    }

    [DisallowMultipleComponent]
    public class EnvironmentManager : MonoBehaviour
    {
        [SerializeField] private Text environmentNameText;
        [SerializeField] private Text environmentDescriptionText;
        [SerializeField] private List<EnvironmentCondition> environments = new List<EnvironmentCondition>
        {
            new EnvironmentCondition
            {
                environmentName = "Full Moon",
                description = "Global Effect: Reversed Arcana are stronger."
            },
            new EnvironmentCondition
            {
                environmentName = "Solar Flare",
                description = "Global Effect: Upright Arcana glow with energy."
            },
            new EnvironmentCondition
            {
                environmentName = "Twilight Fog",
                description = "Global Effect: Orientation is harder to predict."
            }
        };

        private int currentEnvironmentIndex;

        private void Start()
        {
            ShowCurrentEnvironment();
        }

        public void ChangeEnvironment()
        {
            if (environments.Count == 0)
            {
                return;
            }

            currentEnvironmentIndex = (currentEnvironmentIndex + 1) % environments.Count;
            ShowCurrentEnvironment();
        }

        private void ShowCurrentEnvironment()
        {
            if (environments.Count == 0)
            {
                if (environmentNameText != null) environmentNameText.text = "Environment: None";
                if (environmentDescriptionText != null) environmentDescriptionText.text = "No condition configured.";
                return;
            }

            EnvironmentCondition condition = environments[currentEnvironmentIndex];
            if (environmentNameText != null)
            {
                environmentNameText.text = "Environment: " + condition.environmentName;
            }

            if (environmentDescriptionText != null)
            {
                environmentDescriptionText.text = condition.description;
            }
        }
    }
}
