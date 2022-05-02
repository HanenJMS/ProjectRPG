using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Core
{
    public class UnitCore : MonoBehaviour
    {
        [SerializeField] string faction;

        private void Start()
        {
            this.faction = this.gameObject.name;
            UnitCore[] units = FindObjectsOfType<UnitCore>();
        }

        public void SetFaction(string faction)
        {
            this.faction = faction;
        }

        public string GetFaction()
        {
            return this.faction;
        }
        void IsEnemyDetected()
        {
            
        }
    }
}
