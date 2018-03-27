using UnityEngine;
using RPG.Core;
using RPG.Weapons;

namespace RPG.Characters
{
    public class Enemy : MonoBehaviour
    {

        [SerializeField] float chaseRadius = 10f;
        [SerializeField] float attackRadius = 4f;
        [SerializeField] float firingPeriodInSeconds = 0.5f;
        [SerializeField] float firingPeriodVariation = 0.1f;
        [SerializeField] float damagePerShot = 9f;
        [SerializeField] bool isPlayerAttacking;
        [SerializeField] GameObject projectileToUse;
        [SerializeField] GameObject projectileSocket;
        [SerializeField] Vector3 aimOffset = new Vector3(0, 1f, 0);

        Player player = null;
        Enemy enemy;

        bool isAttacking = false;
        
        void Start()
        {
            player = FindObjectOfType<Player>();
            enemy = GetComponent<Enemy>();
        }

        void Update()
        {
            float distanceToPlayer = Vector3.Distance(player.transform.position, transform.position);

            if (distanceToPlayer <= chaseRadius)
            {

            }
            else
            {

            }

            if (distanceToPlayer <= attackRadius && !isAttacking)
            {
                isAttacking = true;
                float randomizedDelay = Random.Range(firingPeriodInSeconds - firingPeriodVariation, firingPeriodInSeconds + firingPeriodVariation);
                InvokeRepeating("FireProjectile", 0f, randomizedDelay);  // TODO switch to coroutines
            }
            else if (distanceToPlayer >= attackRadius && isAttacking)
            {
                isAttacking = false;
                CancelInvoke("FireProjectile");
            }
        }

        // TODO separate character firing logic into separate class.
        void FireProjectile()
        {
            GameObject newProjectile = Instantiate(projectileToUse, projectileSocket.transform.position, Quaternion.identity);
            Projectile projectileComponent = newProjectile.GetComponent<Projectile>();
            projectileComponent.SetDamage(damagePerShot);
            projectileComponent.SetShooter(gameObject);

            Vector3 unitVectorToPlayer = (player.transform.position + aimOffset - projectileSocket.transform.position).normalized;
            float projectileSpeed = projectileComponent.GetDefaultLaunchSpeed();
            newProjectile.GetComponent<Rigidbody>().velocity = unitVectorToPlayer * projectileSpeed;
        }

        void OnTriggerEnter(Collider other)
        {
            if (player.GetComponent<Character>().Attacking)
            {
                var enemyHealth = enemy.GetComponent<Health>();
                enemyHealth.TakeDamage(player.CalculateAttackDamage());
            }
        }

        void OnDrawGizmos()
        {
            // Draw movement gizmos
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position, chaseRadius);

            // Draw attack sphere
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, attackRadius);
        }
    }
}