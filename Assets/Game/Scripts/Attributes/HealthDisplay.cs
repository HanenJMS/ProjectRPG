using System;
using TMPro;
using UnityEngine;

namespace RPG.Attributes
{
    public class HealthDisplay : MonoBehaviour //Consider extending this into the UI system
    {
        Health health;
        private void Awake()
        {
            health = GameObject.FindGameObjectWithTag("Player").GetComponent<Health>();
        }
        private void Update()
        {
            GetComponent<TextMeshProUGUI>().SetText(String.Format("Health: {0}/{1} {2:0.00}%", health.GetCurrentHealth(), health.GetLevelMaxHealth(), health.GetPercentage()));
        }
    }
}
