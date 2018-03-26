using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// TODO remove this class
namespace RPG.Characters
{
    public class EnemyHealthBar : MonoBehaviour
    {
        RawImage healthBarRawImage = null;
        Enemy enemy = null;

        // Use this for initialization
        void Start()
        {
            enemy = GetComponentInParent<Enemy>(); // Different to way player's health bar finds player
            healthBarRawImage = GetComponent<RawImage>();
        }

    }
}
