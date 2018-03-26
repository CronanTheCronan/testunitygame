﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.Characters
{
    public class SpecialAbilities : MonoBehaviour
    {
        public AbilityConfig[] abilities;
        [SerializeField] Image staminaBar;
        [SerializeField] float maxStaminaPoints = 100f;
        [SerializeField] float regenPointsPerSeconds = 1f;
        // todo add outOfEnergy sound;

        float currentStaminaPoints;
        AudioSource audioSource;

        public float staminaAsPercentage { get { return currentStaminaPoints / maxStaminaPoints; } }

        void Start()
        {
            audioSource = GetComponent<AudioSource>();
            SetCurrentMaxStamina();
            UpdateStaminaBar();
            AttachInitialAbilities();
        }

        void Update()
        {
            if(currentStaminaPoints < maxStaminaPoints)
            {
                AddStaminaPoints();
                UpdateStaminaBar();
            }
        }

        void UpdateStaminaBar()
        {
            if (staminaBar)
            {
                staminaBar.fillAmount = staminaAsPercentage;
            }
        }

        void SetCurrentMaxStamina()
        {
            currentStaminaPoints = maxStaminaPoints;
        }

        private void AddStaminaPoints()
        {
            var pointsToAdd = regenPointsPerSeconds * Time.deltaTime;
            currentStaminaPoints = Mathf.Clamp(currentStaminaPoints + pointsToAdd, 0, maxStaminaPoints);
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

        public void AttachInitialAbilities()
        {
            for (int i = 0; i < abilities.Length; i++)
            {
                abilities[i].AddAbilityComponent(gameObject);
            }
        }

        public void AttemptSpecialAbility(int abilityIndex)
        {
            var staminaCost = abilities[abilityIndex].GetStaminaCost();

            if (staminaCost <= currentStaminaPoints)
            {
                var player = GetComponent<Player>();
                ConsumeEnergy(staminaCost);
                var abilityParams = new AbilityUseParams(player.GetBaseDamage());
                abilities[abilityIndex].Use(abilityParams);
            }
            else
            {
                // TODO play out of stamina sound
            }
        }
    }
}
