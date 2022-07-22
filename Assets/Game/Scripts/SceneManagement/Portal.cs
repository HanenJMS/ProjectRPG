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
            SavingWrapper savingWrapper = FindObjectOfType<SavingWrapper>();
            if (sceneToLoad < 0)
            {
                Debug.LogError("Scene to load not set.");
                yield break;
            }
            DontDestroyOnLoad(gameObject);
            Fader fader = FindObjectOfType<Fader>();
            PlayerController player = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
            player.enabled = false;
            yield return fader.FadeOut(fadeOutTime);
            yield return SceneManager.LoadSceneAsync(sceneToLoad);
            PlayerController newPlayer = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
            newPlayer.enabled = false;
            Portal otherPortal = GetOtherPortal();
            UpdatePlayer(otherPortal);
            yield return new WaitForSeconds(fadeInTime);
            PlayerController newPlayer2 = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
            newPlayer2.enabled = false;
            fader.FadeIn(fadeInTime);
            Destroy(gameObject);
            newPlayer2.enabled = true;
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
