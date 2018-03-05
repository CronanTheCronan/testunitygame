﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour, IDamagable {

    [SerializeField] int enemyLayer = 9;
    [SerializeField] float maxHealthPoints = 100f;
    [SerializeField] float damagePerHit = 10f;
    [SerializeField] float minTimeBetweenHits = .5f;
    [SerializeField] float maxAttackRange = 2f;

    CameraRaycaster cameraRaycaster = null;
    GameObject currentTarget;
    float lastHitTime = 0f;

    private float currentHealthPoints;

    public float healthAsPercentage { get { return currentHealthPoints / maxHealthPoints; } }

    void Start()
    {
        cameraRaycaster = FindObjectOfType<CameraRaycaster>();
        cameraRaycaster.notifyMouseClickObservers += OnMouseClick;
        currentHealthPoints = maxHealthPoints;
    }

    void OnMouseClick(RaycastHit raycastHit, int layerHit)
    {
        if(layerHit == enemyLayer)
        {
            var enemy = raycastHit.collider.gameObject;

            // Check enemy is in range
            if ((enemy.transform.position - transform.position).magnitude > maxAttackRange)
            {
                return;
            }

            currentTarget = enemy;

            var enemyComponent = enemy.GetComponent<Enemy>();
            if (Time.time - lastHitTime > minTimeBetweenHits)
            {
                enemyComponent.TakeDamage(damagePerHit);
                lastHitTime = Time.time;
            }
        }
    }


    public void TakeDamage(float damage)
    {
        currentHealthPoints = Mathf.Clamp(currentHealthPoints - damage, 0f, maxHealthPoints);
    }
}
