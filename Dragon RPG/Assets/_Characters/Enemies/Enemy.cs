using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.ThirdPerson;

// TODO consider re-wire
using RPG.Core;
using RPG.Weapons;

namespace RPG.Characters
{
    public class Enemy : MonoBehaviour, IDamagable
    {

        [SerializeField] float maxHealthPoints = 100f;
        [SerializeField] float chaseRadius = 10f;
        [SerializeField] float attackRadius = 4f;
        [SerializeField] float secondsBetweenShot = 0.5f;
        [SerializeField] float damagePerShot = 9f;
        [SerializeField] GameObject projectileToUse;
        [SerializeField] GameObject projectileSocket;
        [SerializeField] Vector3 aimOffset = new Vector3(0, 1f, 0);

        float playerDamage = 10f;

        float currentHealthPoints;
        bool isAttacking = false;
        AICharacterControl aiCharacterControl = null;
        GameObject player = null;
        CapsuleCollider capsuleCollider;
        Weapon weapon;

        Component playerMovement;
        [SerializeField] bool isPlayerAttacking;

        void Start()
        {
            player = GameObject.FindGameObjectWithTag("Player");
            player.GetComponent<PlayerMovement>().notifyPlayerAttackObservers += PlayerIsAttacking;
            //player.GetComponent<PlayerMovement>().notifyPlayerIdleObservers += PlayerIsIdle;

            aiCharacterControl = GetComponent<AICharacterControl>();
            currentHealthPoints = maxHealthPoints;
        }

        void Update()
        {
            float distanceToPlayer = Vector3.Distance(player.transform.position, transform.position);

            if (distanceToPlayer <= chaseRadius)
            {
                aiCharacterControl.SetTarget(player.transform);
            }
            else
            {
                aiCharacterControl.SetTarget(transform);
            }

            if (distanceToPlayer <= attackRadius && !isAttacking)
            {
                isAttacking = true;
                InvokeRepeating("FireProjectile", 0f, secondsBetweenShot);  // TODO switch to coroutines
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

        public float healthAsPercentage { get { return currentHealthPoints / maxHealthPoints; } }

        public void TakeDamage(float damage)
        {
            currentHealthPoints = Mathf.Clamp(currentHealthPoints - damage, 0f, maxHealthPoints);
            if (currentHealthPoints <= 0) { Destroy(gameObject); }
        }

        //void PlayerIsIdle(bool idleStatus)
        //{
        //    if (idleStatus)
        //    {
        //        isPlayerAttacking = false;
        //    }
        //}

        void PlayerIsAttacking(bool attackStatus)
        {
            if (attackStatus)
            {
                isPlayerAttacking = true;
            }
        }

        void OnTriggerEnter(Collider other)
        {
            //isPlayerAttacking = player.gameObject.GetComponent<PlayerMovement>().Attacking;
            if (isPlayerAttacking)
            {
                print("I hit you for " + playerDamage + " dps");
                TakeDamage(playerDamage);
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