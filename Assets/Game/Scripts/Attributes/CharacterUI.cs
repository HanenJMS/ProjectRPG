using RPG.Core;
using UnityEngine;

namespace RPG.Attributes
{
    public class CharacterUI : MonoBehaviour, ICharacterUI
    {
        private new Camera camera;
        Canvas canvas;
        private void Start()
        {
            camera = Camera.main;
            canvas = GetComponent<Canvas>();
        }
        public void UIFaceCamera()
        {
            canvas.transform.LookAt(camera.transform);
        }
    }
}
