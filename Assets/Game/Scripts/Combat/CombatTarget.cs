using RPG.Attributes;
using RPG.Control;
using UnityEngine;
namespace RPG.Combat
{
    [RequireComponent(typeof(Health))]
    public class CombatTarget : MonoBehaviour, IRaycastable
    {
        public bool HandleRayCast(PlayerController callingController)
        {
            if (!callingController.GetComponent<Fighter>().CanAttack(this.gameObject)) return false;
            if (Input.GetMouseButton(1))
            {
                callingController.GetComponent<Fighter>().Attack(this.gameObject);
                print($"{gameObject.name} is currently attack {this.gameObject.name}.");
            }
            return true;
        }
    }
}
