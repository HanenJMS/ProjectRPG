using UnityEngine;

namespace RPG.Attributes
{
    public class HealthBar : MonoBehaviour
    {
        Transform healthBar;
        float maxScale = 3f;
        float maxWidthScale = 0.1f;
        Health health;
        Color defaultColor;
        Camera camera;
        private void Awake()
        {
            healthBar = GetComponent<Transform>();
            health = GetComponentInParent<Health>();
            defaultColor = gameObject.GetComponent<MeshRenderer>().material.color;
            
        }
        private void Start()
        {
            camera = Camera.main;
        }
        private void Update()
        {
            healthBar.localScale = GetHealthSize(health.GetPercentage());
            if (health.GetPercentage() <= 10)
            {
                SetColor(Color.red);
            }
            else if (health.GetPercentage() <= 50)
            {
                SetColor(Color.yellow);
            }
            else
            {
                SetColor(defaultColor);
            }
            if (health.GetPercentage() <= 0)
            {
                gameObject.SetActive(false);
            }
            healthBar.LookAt(camera.transform);
        }

        private Vector3 GetHealthSize(float health)
        {
            if ((health * maxScale) / 100 <= healthBar.localScale.z)
            {
                return new Vector3(maxWidthScale, healthBar.localScale.y, (health * maxScale) / 100);
            }
            return new Vector3((health * maxScale) / 100, healthBar.localScale.y, maxWidthScale);
        }
        private void SetColor(Color color)
        {
            GetComponent<MeshRenderer>().material.color = color;
        }
    }
}