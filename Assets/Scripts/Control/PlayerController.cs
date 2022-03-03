using RPG.Combat;
using RPG.Movement;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace RPG.Control
{
    public class PlayerController : MonoBehaviour
    {
        
        private void MoveToCursor()
        {
            
        }

        private static Ray GetMouseRay()
        {
            return Camera.main.ScreenPointToRay(Input.mousePosition);
        }

        private void Update()
        {
            if (InteractWithCombat()) return;
            else if (InteractWithMovement()) return;
            else
                print("Nothing to do.");
            
        }

        private bool InteractWithCombat()
        {
            RaycastHit[] hits = Physics.RaycastAll(GetMouseRay());
            foreach (RaycastHit hit in hits)
            {
                CombatTarget target = hit.transform.GetComponent<CombatTarget>();
                if (!GetComponent<Fighter>().CanAttack(target)) continue;
                if (target != null)
                {
                    if (Input.GetMouseButtonDown(1))
                    {
                        GetComponent<Fighter>().Attack(target);
                    }
                    return true;
                }
            }
            return false;
        }

        private bool InteractWithMovement()
        {
            RaycastHit hit;
            bool hasHit = Physics.Raycast(GetMouseRay(), out hit);
            if (hasHit)
            {
                if (Input.GetMouseButton(1))
                {
                    this.gameObject.GetComponent<Mover>().StartMoveAction(hit.point);
                    print("Running to point");
                }
                return true;
            }
            return false;
        }
    }
}