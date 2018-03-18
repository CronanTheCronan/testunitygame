using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.Characters
{
    [RequireComponent(typeof(Image))]
    public class PlayerStaminaBar : MonoBehaviour
    {
        Image staminaBarImage;
        Stamina stamina;

        // Use this for initialization
        void Start()
        {
            stamina = FindObjectOfType<Stamina>();
            staminaBarImage = GetComponent<Image>();
        }

        // Update is called once per frame
        void Update()
        {
            staminaBarImage.fillAmount = stamina.staminaAsPercentage;
        }
    }
}
