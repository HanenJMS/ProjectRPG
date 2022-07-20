using UnityEngine;
using UnityEngine.UI;

namespace RPG.Attributes
{
    public class HealthBar : MonoBehaviour
    {
        [SerializeField] RectTransform healthBar = null;
        [SerializeField] Health health = null;
        [SerializeField] Image defaultImage = null;
        float maxScale = 1f;
        Color defaultColor;
        private void Awake()
        {

        }
        private void Start()
        {
            defaultColor = defaultImage.color;
        }
        private void Update()
        {
            healthBar.localScale = GetHealthSize(health.GetPercentage());
            if (health.GetPercentage() <= 20)
            {
                defaultImage.color = Color.red;
            }
            else if (health.GetPercentage() <= 50)
            {
                defaultImage.color = (Color.yellow);
            }
            else
            {
                defaultImage.color = defaultColor;
            }
            if (health.GetPercentage() <= 0)
            {
                gameObject.SetActive(false);
                return;
            }
        }
        private Vector3 GetHealthSize(float health)
        {
            return new Vector3((health / 100 * maxScale), maxScale, maxScale);
        }
    }
}
