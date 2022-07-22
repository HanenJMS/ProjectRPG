using UnityEngine;

namespace RPG.Core
{
    public class FacingCamera : MonoBehaviour
    {
        void LateUpdate()
        {
            transform.forward = Camera.main.transform.forward;
        }
    }
}