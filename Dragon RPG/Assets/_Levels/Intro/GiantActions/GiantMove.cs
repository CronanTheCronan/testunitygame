using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;
using UnityStandardAssets.Characters.ThirdPerson;
using UnityEngine;

namespace RPG.Levels
{
    public class GiantMove : MonoBehaviour
    {
        [SerializeField]
        float giantTargetRadius = 2.0f;
        GameObject giantTarget = null;
        GameObject giantClone = null;
        private ThirdPersonCharacter character;
        private bool hasMoved = false;
        private NavMeshAgent agent;


        // Use this for initialization
        void Start()
        {
            giantTarget = GameObject.FindGameObjectWithTag("GiantTarget");
            agent = GetComponent<NavMeshAgent>();
        }

        // Update is called once per frame
        void Update()
        {
            float distanceToPlayer = Vector3.Distance(transform.position, giantTarget.transform.position);
            if (distanceToPlayer <= giantTargetRadius)
            {
                print("Giant has reached destination");
                DestroyGameObject();
            }

            if (!hasMoved)
            {
                GiantMoving();

            }


        }

        void GiantMoving()
        {
            //agent.SetDestination(giantTarget.transform.position);
            //gameObject.GetComponent<NavMeshAgent>().autoBraking = false;
            GetComponent<NavMeshAgent>().destination = giantTarget.transform.position;
            //GetComponent<ThirdPersonCharacter>().Move(giantTarget.transform.position, false, false);
            if (transform.position == giantTarget.transform.position)
            {
                hasMoved = true;
            }
        }

        void DestroyGameObject()
        {
            Destroy(gameObject);
        }

        void OnDrawGizmos()
        {
            // Draw movement gizmos
            Gizmos.color = Color.white;
            Gizmos.DrawWireSphere(giantTarget.transform.position, giantTargetRadius);
        }
    }
}
