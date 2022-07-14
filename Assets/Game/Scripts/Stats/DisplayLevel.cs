using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace RPG.Stats
{
    public class DisplayLevel : MonoBehaviour
    {
        BaseStats stats;
        private void Awake()
        {
            stats = GameObject.FindGameObjectWithTag("Player").GetComponent<BaseStats>();
        }
        private void OnEnable()
        {
            if(stats != null)
            {
                stats.OnLevelUp += UpdateLevelDisplay;
            }
        }
        private void OnDisable()
        {
            if (stats != null)
            {
                stats.OnLevelUp -= UpdateLevelDisplay;
            }
        }
        private void Start()
        {
            
            UpdateLevelDisplay();
        }
        private void UpdateLevelDisplay()
        {
            print("Calculating. Debugging for loop. Inside DislayLevel.cs UI");
            GetComponent<TextMeshProUGUI>().SetText(String.Format("Level: {0:0}", stats.GetLevel()));
        }
    }
}
