using RPG.Control;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

namespace RPG.SceneManagement
{
    public class Portal : MonoBehaviour
    {
        enum DesinationIdentifier
        {
            A, B, C, D, E, F
        }
        [SerializeField] int sceneToLoad = -1;
        [SerializeField] Transform spawnPoint;
        [SerializeField] DesinationIdentifier destination;
        [SerializeField] float fadeOutTime = 1f;
        [SerializeField] float fadeInTime = 2f;
        [SerializeField] float fadeWaitTime = 0.5f;
        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.name == "Player")
            {
                StartCoroutine(Transition());
            }
        }
        private IEnumerator Transition()
        {
            Fader fader = FindObjectOfType<Fader>();
            SavingWrapper savingWrapper = FindObjectOfType<SavingWrapper>();
            PlayerController player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
            player.enabled = false;
            DontDestroyOnLoad(gameObject);
            yield return fader.FadeOut(fadeOutTime);
            savingWrapper.Save();
            yield return SceneManager.LoadSceneAsync(sceneToLoad);
            PlayerController newPlayer = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
            newPlayer.enabled = false;
            savingWrapper.Load();
            Portal otherPortal = GetOtherPortal();
            UpdatePlayer(otherPortal);
            savingWrapper.Save();
            yield return new WaitForSeconds(fadeWaitTime);
            yield return fader.FadeIn(fadeInTime);
            newPlayer.enabled = true;
            Destroy(gameObject);
        }

        private Portal GetOtherPortal()
        {
            foreach (Portal portal in FindObjectsOfType<Portal>())
            {
                if (portal == this) continue;
                if (this.destination != portal.destination) continue;
                return portal;
            }
            return null;
        }

        private void UpdatePlayer(Portal otherPortal)
        {
            foreach (GameObject player in GameObject.FindGameObjectsWithTag("unit"))
            {
                if (player.name == "Player")
                {
                    player.GetComponent<NavMeshAgent>().enabled = false;
                    player.GetComponent<NavMeshAgent>().Warp(otherPortal.spawnPoint.position);
                    player.transform.rotation = otherPortal.spawnPoint.rotation;
                    player.GetComponent<NavMeshAgent>().enabled = true;
                    return;
                }
            }
        }
    }
}
