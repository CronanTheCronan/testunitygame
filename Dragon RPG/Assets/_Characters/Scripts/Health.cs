using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace RPG.Characters
{
    public class Health : MonoBehaviour
    {

        const string DEATH_TRIGGER = "Death";

        [SerializeField] float maxHealthPoints = 100f;
        [SerializeField] Image healthBar;
        [SerializeField] AudioClip[] damageSounds;
        [SerializeField] AudioClip[] deathSounds;
        [SerializeField] float deathVanishSeconds = 2.0f;

        Animator animator;
        AudioSource audioSource;
        CharacterMovement characterMovement;

        private float currentHealthPoints;

        public float healthAsPercentage { get { return currentHealthPoints / maxHealthPoints; } }

        // TODO maybe a parameter for character disappearing

        void Start()
        {
            animator = GetComponent<Animator>();
            audioSource = GetComponent<AudioSource>();
            characterMovement = GetComponent<CharacterMovement>();

            SetCurrentMaxHealth();
            currentHealthPoints = maxHealthPoints;
        }

        // Update is called once per frame
        void Update()
        {
            UpdateHealthBar();
        }

        private void UpdateHealthBar()
        {
            if(healthBar) //Enemies may not have health bars to update
            {
                healthBar.fillAmount = healthAsPercentage;
            }
        }


        private void SetCurrentMaxHealth()
        {
            currentHealthPoints = maxHealthPoints;
        }

        public void TakeDamage(float damage)
        {
            bool characterDies = currentHealthPoints - damage <= 0;

            currentHealthPoints = Mathf.Clamp(currentHealthPoints - damage, 0f, maxHealthPoints);
            var clip = damageSounds[UnityEngine.Random.Range(0, damageSounds.Length)];
            audioSource.PlayOneShot(clip);

            if (characterDies)
            {
                StartCoroutine(KillCharacter());
            }
        }

        IEnumerator KillCharacter()
        {
            StopAllCoroutines();
            characterMovement.Kill();
            animator.SetTrigger(DEATH_TRIGGER);

            var playerComponent = GetComponent<Player>();
            if(playerComponent && playerComponent.isActiveAndEnabled)
            {
                audioSource.clip = deathSounds[UnityEngine.Random.Range(0, deathSounds.Length)];
                audioSource.Play();
                yield return new WaitForSecondsRealtime(audioSource.clip.length + 1f);

                SceneManager.LoadScene(0);
            }
            else // assume enemy for now, otherwise reconisder for NPC's
            {
                DestroyObject(gameObject, deathVanishSeconds);
            }
        }

        public void Heal(float points)
        {
            currentHealthPoints = Mathf.Clamp(currentHealthPoints + points, 0f, maxHealthPoints);
        }
    }
}
