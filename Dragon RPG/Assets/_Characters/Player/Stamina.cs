using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.Characters
{
    public class Stamina : MonoBehaviour
    {
        [SerializeField] float maxStaminaPoints = 100f;
        [SerializeField] float regenPointsPerSeconds = 1f;

        float currentStaminaPoints;

        public float staminaAsPercentage { get { return currentStaminaPoints / maxStaminaPoints; } }

        void Start()
        {
            currentStaminaPoints = maxStaminaPoints;
        }

        void Update()
        {
            if(currentStaminaPoints < maxStaminaPoints)
            {
                AddStaminaPoints();
            }
        }

        private void AddStaminaPoints()
        {
            var pointsToAdd = regenPointsPerSeconds * Time.deltaTime;
            currentStaminaPoints = Mathf.Clamp(currentStaminaPoints + pointsToAdd, 0, maxStaminaPoints);
        }

        public bool IsStaminaAvailable(float amount)
        {
            return amount <= currentStaminaPoints;
        }

        public void ConsumeEnergy(float amount)
        {
            float newStaminaPoints = currentStaminaPoints - amount;
            currentStaminaPoints = Mathf.Clamp(newStaminaPoints, 0, maxStaminaPoints);
        }

        private float StaminaAsPercent()
        {
            return currentStaminaPoints / maxStaminaPoints;
        }
    }
}
