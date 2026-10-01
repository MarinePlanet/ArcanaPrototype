using System;
using UnityEngine;

namespace ArcanaPrototype
{
    [DisallowMultipleComponent]
    public class PlayerHealth : MonoBehaviour
    {
        [Header("Health")]
        [SerializeField, Min(1)] private int maximumHealth = 100;
        [SerializeField] private HealthBarView healthBarView;

        [Header("Inspector Testing")]
        [SerializeField, Min(1)] private int testAmount = 10;

        public int CurrentHealth { get; private set; }
        public int MaximumHealth => maximumHealth;
        public event Action HealthDepleted;

        private bool hasReportedDepletion;

        private void Awake()
        {
            CurrentHealth = maximumHealth;
            hasReportedDepletion = false;
            RefreshDisplay();
        }

        public void TakeDamage(int amount)
        {
            int damage = Mathf.Max(0, amount);
            CurrentHealth = Mathf.Clamp(CurrentHealth - damage, 0, maximumHealth);
            RefreshDisplay();
            ReportDepletionIfNeeded();
        }

        public void Heal(int amount)
        {
            int healing = Mathf.Max(0, amount);
            CurrentHealth = Mathf.Clamp(CurrentHealth + healing, 0, maximumHealth);
            RefreshDisplay();
        }

        [ContextMenu("Reset To Full Health")]
        public void ResetToFullHealth()
        {
            CurrentHealth = maximumHealth;
            hasReportedDepletion = false;
            RefreshDisplay();
        }

        [ContextMenu("Test: Take Damage")]
        private void TestTakeDamage()
        {
            TakeDamage(testAmount);
        }

        [ContextMenu("Test: Heal")]
        private void TestHeal()
        {
            Heal(testAmount);
        }

        private void OnValidate()
        {
            maximumHealth = Mathf.Max(1, maximumHealth);
            testAmount = Mathf.Max(1, testAmount);

            if (Application.isPlaying)
            {
                CurrentHealth = Mathf.Clamp(CurrentHealth, 0, maximumHealth);
                RefreshDisplay();
            }
        }

        private void RefreshDisplay()
        {
            if (healthBarView != null)
            {
                healthBarView.SetHealth(CurrentHealth, maximumHealth);
            }
        }

        private void ReportDepletionIfNeeded()
        {
            if (CurrentHealth > 0 || hasReportedDepletion)
            {
                return;
            }

            hasReportedDepletion = true;
            HealthDepleted?.Invoke();
        }
    }
}
