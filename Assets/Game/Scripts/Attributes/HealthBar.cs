using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Attributes
{
    public class HealthBar : MonoBehaviour
    {
        Transform transform;
        float maxScale = 3f;
        float maxWidthScale = 0.1f;
        Health health;
        Color defaultColor;
        private void Start()
        {
            transform = GetComponent<Transform>();
            health = GetComponentInParent<Health>();
            defaultColor = gameObject.GetComponent<MeshRenderer>().material.color;
        }
        private void Update()
        {
            transform.localScale = GetHealthSize(health.GetPercentage());
            if(health.GetPercentage() <= 10)
            {
                SetColor(Color.red);
            }
            else if(health.GetPercentage() <= 50)
            {
                SetColor(Color.yellow);
            }
            else
            {
                SetColor(defaultColor);
            }
            if(health.GetPercentage() <= 0)
            {
                gameObject.SetActive(false);
            }
        }

        private Vector3 GetHealthSize(float health)
        {
            if((health * maxScale) / 100 <= transform.localScale.z)
            {
                return new Vector3(maxWidthScale, transform.localScale.y, (health * maxScale) / 100);
            }
            return new Vector3((health * maxScale)/100, transform.localScale.y, maxWidthScale);
        }
        private void SetColor(Color color)
        {
            GetComponent<MeshRenderer>().material.color = color;
        }
    }
}