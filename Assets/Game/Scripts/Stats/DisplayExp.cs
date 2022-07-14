using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace RPG.Stats
{
    public class DisplayExp : MonoBehaviour
    {
        Experience exp;
        private void Awake()
        {
            exp = GameObject.FindGameObjectWithTag("Player").GetComponent<Experience>();
        }
        private void OnEnable()
        {
            if (exp != null)
                exp.OnExperenceGained += UpdateExpUI;
        }
        private void OnDisable()
        {
            if (exp != null)
                exp.OnExperenceGained -= UpdateExpUI;
        }
        private void Start()
        {
            UpdateExpUI();
        }
        private void UpdateExpUI()
        {
            GetComponent<TextMeshProUGUI>().SetText(String.Format("Exp: {0:0.00}", exp.GetExperiencePoints()));
        }
    }
}
