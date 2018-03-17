using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.Characters
{
    public class Stamina : MonoBehaviour
    {
        [SerializeField] RawImage staminaBar;
        [SerializeField] float maxStaminaPoints = 100f;
        [SerializeField] float regenPointsPerSeconds = 1f;

        public float currentStaminaPoints;

        void Start()
        {
            currentStaminaPoints = maxStaminaPoints;
        }

        void Update()
        {
            if(currentStaminaPoints < maxStaminaPoints)
            {
                AddStaminaPoints();
                UpdateStaminaBar();
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
            UpdateStaminaBar();
        }

        private void UpdateStaminaBar()
        {
            // TODO remove magic numbers;
            float xValue = -(StaminaAsPercent() / 2f) - 0.5f;
            staminaBar.uvRect = new Rect(xValue, 0f, 0.5f, 1f);
        }

        private float StaminaAsPercent()
        {
            return currentStaminaPoints / maxStaminaPoints;
        }
    }
}
