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
        private void Start()
        {
            exp = GameObject.FindGameObjectWithTag("Player").GetComponent<Experience>();
            exp.OnExperenceGained += UpdateExpUI;
            UpdateExpUI();
        }
        private void UpdateExpUI()
        {
            GetComponent<TextMeshProUGUI>().SetText(String.Format("Exp: {0:0.00}", exp.GetExperiencePoints()));
        }
    }
}
