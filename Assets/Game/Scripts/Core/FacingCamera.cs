using UnityEngine;

namespace RPG.Core
{
    public class FacingCamera : MonoBehaviour
    {
        void Update()
        {
            transform.forward = Camera.main.transform.forward;
        }
    }
}