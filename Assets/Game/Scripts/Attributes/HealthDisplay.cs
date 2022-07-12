using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.Attributes
{
    public class HealthDisplay : MonoBehaviour //Consider extending this into the UI system
    {
        Health health;
        private void Start()
        {
            health = GameObject.FindGameObjectWithTag("Player").GetComponent<Health>();
        }
        private void Update()
        {
            GetComponent<TextMeshProUGUI>().SetText(String.Format("Health: {0:0.00}%", health.GetPercentage()));
        }
    }
}
