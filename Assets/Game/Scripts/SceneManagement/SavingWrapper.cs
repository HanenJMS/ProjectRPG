using RPG.Saving;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.SceneManagement
{
    public class SavingWrapper : MonoBehaviour
    {
        const string defaultSaveFile = "save";
        [SerializeField] float fadeInTime = 0.5f;
        private void Awake()
        {
            StartCoroutine(LoadLastScene());
        }
        private IEnumerator LoadLastScene()
        {
            Fader fade = FindObjectOfType<Fader>();
            fade.FadeOutImmediate();
            yield return GetComponent<SavingSystem>().LoadLastScene(defaultSaveFile);
            yield return fade.FadeIn(fadeInTime);
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.F5))
            {
                Save();
            }
            if (Input.GetKeyDown(KeyCode.F2))
            {
                Load();
            }
            if(Input.GetKeyDown(KeyCode.F3))
            {
                Delete();
            }
        }
        
        public void Load()
        {
            GetComponent<SavingSystem>().Load(defaultSaveFile);
        }

        public void Save()
        {
            GetComponent<SavingSystem>().Save(defaultSaveFile);
        }
        public void Delete()
        {
            GetComponent<SavingSystem>().Delete(defaultSaveFile);
        }
    }
}
