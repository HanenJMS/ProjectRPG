using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.SceneManagement
{
    public class Fader : MonoBehaviour
    {
        CanvasGroup canvasGroup;
        private void Awake()
        {
            canvasGroup = GetComponent<CanvasGroup>();
        }
        public void FadeOutImmediate()
        {
            while (canvasGroup.alpha < 1)
            {
                canvasGroup.alpha = 1;
            }
        }
        public IEnumerator FadeOut(float time)
        {
            while(canvasGroup.alpha < 1)
            {
                canvasGroup.alpha += 1 / (time / Time.deltaTime);
                yield return null;
            }
        }
        public IEnumerator FadeIn(float time)
        {
            while (canvasGroup.alpha > 0)
            {
                canvasGroup.alpha -= 1 / (time / Time.deltaTime);
                yield return null;
            }
        }
    }
}
