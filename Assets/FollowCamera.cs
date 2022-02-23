using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    [SerializeField] private GameObject target;
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            FollowTarget();
        }
        this.transform.position = target.transform.position;
    }

    private void FollowTarget()
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
