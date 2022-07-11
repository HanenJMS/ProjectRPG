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
        private void Start()
        {
            stats = GameObject.FindGameObjectWithTag("Player").GetComponent<BaseStats>();
        }
        private void Update()
        {
            GetComponent<TextMeshProUGUI>().SetText(String.Format("Level: {0:0}", stats.GetLevel()));
        }
    }
}
