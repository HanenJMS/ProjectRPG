using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Attributes
{
    public class HealthBar : MonoBehaviour
    {
        Transform transform;
        Health health;
        private void Start()
        {
            transform = GetComponent<Transform>();
            health = GetComponentInParent<Health>();
        }
        private void Update()
        {
            transform.localScale = GetHealthSize(health.GetPercentage());
        }

        private Vector3 GetHealthSize(float health)
        {
            return new Vector3(health / 100f, 0.1f, 0.01f);
        }
    }
}