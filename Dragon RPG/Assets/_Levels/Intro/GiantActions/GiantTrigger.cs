using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.ThirdPerson;
using UnityEngine.AI;

namespace RPG.Levels
{
    public class GiantTrigger : MonoBehaviour
    {
        [SerializeField] float triggerRadius = .5f;
        GameObject player = null;
        public GameObject Giant;
        GameObject enemySpawner = null;
        bool alreadySpawned = false;

        // Use this for initialization
        void Start()
        {
            player = GameObject.FindGameObjectWithTag("Player");
            enemySpawner = GameObject.FindGameObjectWithTag("EnemySpawner");
        }

        void Update()
        {
            float distanceToPlayer = Vector3.Distance(player.transform.position, transform.position);
            if (distanceToPlayer < triggerRadius && !alreadySpawned)
            {
                Instantiate(Giant, enemySpawner.transform.position, Quaternion.identity);
                alreadySpawned = true;
            }
        }


        void OnDrawGizmos()
        {
            // Draw movement gizmos
            Gizmos.color = Color.white;
            Gizmos.DrawWireSphere(transform.position, triggerRadius);
        }
    }
}
