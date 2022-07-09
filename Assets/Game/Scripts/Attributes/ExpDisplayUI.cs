using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace RPG.Attributes
{
    public class ExpDisplayUI : MonoBehaviour
    {
        Experience exp;
        private void Awake()
        {
            exp = GameObject.FindGameObjectWithTag("Player").GetComponent<Experience>();
        }
        private void Update()
        {
            GetComponent<TextMeshProUGUI>().SetText(String.Format("Exp: {0:0.00}%", exp.GetExperience()));
        }
    }
}
