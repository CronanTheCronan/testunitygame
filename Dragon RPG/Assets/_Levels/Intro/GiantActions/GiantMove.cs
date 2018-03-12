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
        GameObject giantTargetBridge = null;
        GameObject giantTargetEnd = null;
        ThirdPersonCharacter character;
        NavMeshAgent agent;
        private bool hasReachedBridge = false;
        private bool hasReachedEndGoal = false;
        private float distanceToTarget;


        // Use this for initialization
        void Start()
        {
            giantTargetBridge = GameObject.FindGameObjectWithTag("GiantTargetBridge");
            //giantTargetEnd = GameObject.FindGameObjectWithTag("GiantTarget");
            agent = GetComponent<NavMeshAgent>();
            
        }

        // Update is called once per frame
        void FixedUpdate()
        {
            print(hasReachedBridge);
            print(hasReachedEndGoal);
            if (!hasReachedBridge)
            {
                GiantMoveToBridge();
            }
            if (hasReachedBridge && !hasReachedEndGoal)
            {
                GiantMoveToEnd();
            }
            //if(!hasReachedEndGoal)
            //{
            //    if(!hasReachedBridge)
            //    {
            //        GiantMoveToBridge();
            //    }
            //    else
            //    {
            //        GiantMoveToEnd();
            //    }
            //}
        }

        void GiantMoveToBridge()
        {
            float distanceToTarget = Vector3.Distance(transform.position, giantTargetBridge.transform.position);
            agent.destination = giantTargetBridge.transform.position;
            if (distanceToTarget <= giantTargetRadius)
            {
                hasReachedBridge = true;
            }
        }

        void GiantMoveToEnd()
        {
            float distanceToEndTarget = Vector3.Distance(transform.position, giantTargetEnd.transform.position);
            print("Distance to end: " + distanceToEndTarget);
            agent.destination = giantTargetEnd.transform.position;
            if (distanceToEndTarget <= giantTargetRadius)
            {
                hasReachedEndGoal = true;
                Destroy(gameObject);
            }
        }

    }
}
