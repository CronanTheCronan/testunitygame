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
        [SerializeField] float energyUsed = 10f;

        PlayerMovement playerMovement;

        float currentStaminaPoints;

        // Use this for initialization
        void Start()
        {
            playerMovement = GetComponent<PlayerMovement>();
            playerMovement.notifyPlayerAttackObservers += PlayerAttacking;
            currentStaminaPoints = maxStaminaPoints;
        }

        void PlayerAttacking(bool attackStatus)
        {
            if(attackStatus)
            {
                currentStaminaPoints = Mathf.Clamp(currentStaminaPoints - energyUsed, 0f, maxStaminaPoints);

                UpdateStaminaBar();
            }
        }

        private void UpdateStaminaBar()
        {
            float xValue = -(StaminaAsPercent() / 2f) - 0.5f;
            staminaBar.uvRect = new Rect(xValue, 0f, 0.5f, 1f);
        }

        private float StaminaAsPercent()
        {
            return currentStaminaPoints / maxStaminaPoints;
        }
    }
}
