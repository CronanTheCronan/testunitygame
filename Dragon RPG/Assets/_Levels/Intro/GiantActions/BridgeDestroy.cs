using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Levels
{
    public class BridgeDestroy : MonoBehaviour
    {

        GameObject giant = null;
        // Use this for initialization
        void Start()
        {
            giant = GameObject.FindGameObjectWithTag("Giant");
        }

        // Update is called once per frame
        void Update()
        {

        }

        void OnCollisionEnter(Collision other)
        {
            if (giant)
                Destroy(gameObject);
        }
    }
}
