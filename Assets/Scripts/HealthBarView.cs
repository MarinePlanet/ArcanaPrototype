using UnityEngine;
using UnityEngine.UI;

namespace ArcanaPrototype
{
    [DisallowMultipleComponent]
    public class HealthBarView : MonoBehaviour
    {
        [Header("UI References")]
        [SerializeField] private Image fillImage;
        [SerializeField] private Text healthText;

        private void Awake()
        {
            ConfigureFillImage();
        }

        public void SetHealth(int currentHealth, int maximumHealth)
        {
            int safeMaximum = Mathf.Max(1, maximumHealth);
            int safeCurrent = Mathf.Clamp(currentHealth, 0, safeMaximum);

            if (fillImage != null)
            {
                fillImage.fillAmount = (float)safeCurrent / safeMaximum;
            }

            if (healthText != null)
            {
                healthText.text = safeCurrent + " / " + safeMaximum;
            }
        }

        private void ConfigureFillImage()
        {
            if (fillImage == null)
            {
                return;
            }

            fillImage.type = Image.Type.Filled;
            fillImage.fillMethod = Image.FillMethod.Horizontal;
            fillImage.fillOrigin = (int)Image.OriginHorizontal.Left;
        }
    }
}
