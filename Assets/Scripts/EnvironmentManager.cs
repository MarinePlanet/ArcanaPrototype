using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ArcanaPrototype
{
    [DisallowMultipleComponent]
    public class EnvironmentManager : MonoBehaviour
    {
        [SerializeField] private Text environmentNameText;
        [SerializeField] private Text environmentDescriptionText;
        [SerializeField] private List<EnvironmentData> environments = new List<EnvironmentData>();

        private int currentEnvironmentIndex;

        public EnvironmentData CurrentEnvironment => environments.Count > 0
            ? environments[currentEnvironmentIndex]
            : null;

        private void Start()
        {
            ResetEnvironment();
        }

        public void ResetEnvironment()
        {
            currentEnvironmentIndex = 0;
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

        public bool ChangeToRandomDifferentEnvironment()
        {
            if (environments.Count <= 1)
            {
                return false;
            }

            int offset = Random.Range(1, environments.Count);
            currentEnvironmentIndex = (currentEnvironmentIndex + offset) % environments.Count;
            ShowCurrentEnvironment();
            return true;
        }

        private void ShowCurrentEnvironment()
        {
            if (environments.Count == 0)
            {
                if (environmentNameText != null) environmentNameText.text = "Environment: None";
                if (environmentDescriptionText != null) environmentDescriptionText.text = "No condition configured.";
                return;
            }

            EnvironmentData condition = environments[currentEnvironmentIndex];
            if (condition == null)
            {
                if (environmentNameText != null) environmentNameText.text = "Environment: Missing Data";
                if (environmentDescriptionText != null) environmentDescriptionText.text = "Assign an EnvironmentData asset.";
                return;
            }

            if (environmentNameText != null)
            {
                environmentNameText.text = "Environment: " + condition.EnvironmentName;
            }

            if (environmentDescriptionText != null)
            {
                environmentDescriptionText.text = condition.Description;
            }
        }
    }
}
