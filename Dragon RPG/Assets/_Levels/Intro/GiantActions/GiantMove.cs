using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;
using UnityStandardAssets.Characters.ThirdPerson;
using UnityEngine;
using System;

namespace RPG.Levels
{
    public class GiantMove : MonoBehaviour
    {
        [SerializeField] float giantTargetRadius = 5.0f;
        [SerializeField] bool hasReachedGoal = false;
        GameObject giantTargetBridge = null;
        GameObject giantTargetEnd = null;
        NavMeshAgent agent;

        // Use this for initialization
        void Start()
        {
            //giantTargetBridge = GameObject.FindGameObjectWithTag("GiantTargetBridge");
            giantTargetEnd = GameObject.FindGameObjectWithTag("GiantTarget");
            agent = GetComponent<NavMeshAgent>();
        }

        // Update is called once per frame
        void Update()
        {
            if (!hasReachedGoal)
            {
                GiantMoving();
            }
        }

        void GiantMoving()
        {
            float distanceToEndTarget = Vector3.Distance(transform.position, giantTargetEnd.transform.position);
            agent.destination = giantTargetEnd.transform.position;
            if (distanceToEndTarget <= giantTargetRadius)
            {
                hasReachedGoal = true;
                Destroy(gameObject);
            }
        }

    }
}
