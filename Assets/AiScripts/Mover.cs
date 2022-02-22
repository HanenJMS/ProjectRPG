using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;

public class Mover : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] GameObject target;
    NavMeshAgent agent;


    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButton(1))
        {
            MoveToCursor();
        }
        
    }

    private void MoveToCursor()
    {
        Ray lastRay;
        RaycastHit hit;
        lastRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        bool hasHit = Physics.Raycast(lastRay, out hit);
        if(hasHit)
        {
            agent.SetDestination(hit.point);
        }
        Debug.DrawRay(lastRay.origin, lastRay.direction * 100);
    }
}
