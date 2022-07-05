using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Combat
{
    public class WeaponPickup : MonoBehaviour
    {
        [SerializeField] Weapon weapon = null;
        private void OnTriggerEnter(Collider other)
        {
            Fighter fighter = other.GetComponent<Fighter>();
            if(fighter != null)
            {
                fighter.EquipWeapon(weapon);
                Destroy(gameObject);
            }
        }
    }
}
