using System;
using System.Collections;
using System.Collections.Generic;
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

        private void OnTriggerEnter(Collider other)
        {
            if(other.gameObject.name == "Player")
            {
                StartCoroutine(Transition());
            }
        }
        private IEnumerator Transition()
        {
            DontDestroyOnLoad(gameObject);
            yield return SceneManager.LoadSceneAsync(sceneToLoad);
            Portal otherPortal = GetOtherPortal();
            UpdatePlayer(otherPortal);
            Destroy(gameObject);
        }

        private Portal GetOtherPortal()
        {
            foreach(Portal portal in FindObjectsOfType<Portal>())
            {
                if (portal == this) continue;
                if (this.destination != portal.destination) continue;
                return portal;
            }
            return null;
        }

        private void UpdatePlayer(Portal otherPortal)
        {
            foreach(GameObject player in GameObject.FindGameObjectsWithTag("unit"))
            {
                if(player.name == "Player")
                {
                    player.GetComponent<NavMeshAgent>().Warp(otherPortal.spawnPoint.position);
                    player.transform.rotation = otherPortal.spawnPoint.rotation;
                    return;
                }
            }
        }
    }
}
