using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace RPG.Saving
{
    public class SavingSystem : MonoBehaviour
    {
        string lastBuildIndex = "lastBuildIndex";
        public IEnumerator LoadLastScene(string saveFile)
        {
            Dictionary<string, object> state = LoadFile(saveFile);
            if(state.ContainsKey(lastBuildIndex))
            {
                int buildIndex = (int)state[lastBuildIndex];
                if (buildIndex != SceneManager.GetActiveScene().buildIndex)
                {
                    yield return SceneManager.LoadSceneAsync(buildIndex);
                }
            }
            RestoreState(state);
        }
        public void Save(string saveFile)
        {
            Dictionary<string, object> state = LoadFile(saveFile);
            CaptureState(state);
            SaveFile(saveFile, state);
        }
        public void Load(string saveFile)
        {
            RestoreState(LoadFile(saveFile));
        }
        public void Delete(string saveFile)
        {
            string path = GetPathFromSaveFile(saveFile);
            print("Deleting " + path);
            File.Delete(path);
        }
        private void SaveFile(string saveFile, object state)
        {
            string path = GetPathFromSaveFile(saveFile); //getting the system path
            print("saving to " + path);
            using (FileStream stream = File.Open(path, FileMode.Create))//opening file
            {
                BinaryFormatter formatter = new BinaryFormatter();
                formatter.Serialize(stream, state); //serializing into file
            }
        }
        private Dictionary<string, object> LoadFile(string saveFile)
        {
            string path = GetPathFromSaveFile(saveFile);
            if(!File.Exists(path))
            {
                return new Dictionary<string, object>();
            }
            using (FileStream stream = File.Open(path, FileMode.Open))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                return (Dictionary<string, object>)formatter.Deserialize(stream);
            }
        }
        private void CaptureState(Dictionary<string, object> state)
        {
            foreach(SaveableEntity saveable in FindObjectsOfType<SaveableEntity>())
            {
                state[saveable.GetUniqueIdentifier()] = saveable.CaptureState();
            }
            state[lastBuildIndex] = SceneManager.GetActiveScene().buildIndex;
        }
        private static void RestoreState(Dictionary<string, object> state)
        {
            foreach (SaveableEntity saveable in FindObjectsOfType<SaveableEntity>())
            {
                string id = saveable.GetUniqueIdentifier();
                if(state.ContainsKey(id))
                {
                    saveable.RestoreState(state[id]);
                }
            }
        }
        private string GetPathFromSaveFile(string saveFile)
        {
            return Path.Combine(Application.persistentDataPath, saveFile + ".sav");
        }
    }
}
