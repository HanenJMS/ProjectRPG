using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Control
{
    public class PatrolPath : MonoBehaviour
    {
        const float waypointGizmoRadius = 0.3f;
        [SerializeField] List<Waypoint> localPatrolPath;

        private void Start()
        {
            localPatrolPath = new List<Waypoint>(FindObjectsOfType<Waypoint>());
        }
        public List<Waypoint> GetWaypoints()
        {
            return localPatrolPath;
        }
        private void OnDrawGizmos()
        {
            for(int i = 0; i < transform.childCount; i++)
            {
                Gizmos.DrawSphere(transform.GetChild(i).position, waypointGizmoRadius); 
            }
        }
    }
}
