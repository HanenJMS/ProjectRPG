using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Core
{
    public class FollowCamera : MonoBehaviour
    {
        [SerializeField] private GameObject target;
        [SerializeField] private float mouseScroll = 5f;
        void LateUpdate()
        {
            //if (Input.GetMouseButtonDown(0))
            //{
            //    SelectTarget();
            //}
            this.transform.position = target.transform.position;
            if (Input.mouseScrollDelta.y != 0)
            {
                Camera.main.fieldOfView -= (Input.mouseScrollDelta.y);
            }
        }

        private void SelectTarget()
        {
            Ray lastRay;
            RaycastHit hit;
            lastRay = Camera.main.ScreenPointToRay(Input.mousePosition);
            bool hasHit = Physics.Raycast(lastRay, out hit);
            if (hasHit)
            {
                if (hit.transform.tag == "unit")
                {
                    target = hit.transform.gameObject;
                }
            }

            Debug.DrawRay(lastRay.origin, lastRay.direction * 100);
        }
    }
}
