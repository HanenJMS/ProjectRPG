using RPG.Attributes;
using RPG.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Attributes
{
    public class CharacterUI : MonoBehaviour, ICharacterUI
    {
        Camera camera;
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
