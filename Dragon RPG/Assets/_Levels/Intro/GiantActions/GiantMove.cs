using UnityEngine.AI;
using UnityEngine;


namespace RPG.Levels
{
    public class GiantMove : MonoBehaviour
    {
        [SerializeField] float giantTargetRadius = 5.0f;
        [SerializeField] bool hasReachedGoal = false;
        GameObject giantTargetEnd = null;
        GameObject giant;
        NavMeshAgent agent;

        // Use this for initialization
        void Start()
        {
            giant = this.gameObject;
            giantTargetEnd = GameObject.FindGameObjectWithTag("GiantTarget");
            agent = GetComponent<NavMeshAgent>();
        }

        void Update()
        {
            giant.transform.LookAt(giantTargetEnd.transform.position);
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
