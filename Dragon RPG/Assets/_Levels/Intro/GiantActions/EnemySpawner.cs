using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Levels
{
    public class EnemySpawner : MonoBehaviour
    {

        void OnDrawGizmos()
        {
            // Draw movement gizmos
            Gizmos.color = Color.white;
            Gizmos.DrawWireSphere(transform.position, .5f);
        }
    }
}
