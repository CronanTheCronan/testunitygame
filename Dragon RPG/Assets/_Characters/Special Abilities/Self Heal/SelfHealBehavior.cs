using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Core;
using System;

namespace RPG.Characters
{
    public class SelfHealBehavior : MonoBehaviour, ISpecialAbility
    {
        SelfHealConfig config = null;
        Player player = null;
        AudioSource audioSource = null;

        public void SetConfig(SelfHealConfig configToSet)
        {
            this.config = configToSet;
        }

        // Use this for initialization
        void Start()
        {
            player = GetComponent<Player>();
            audioSource = GetComponent<AudioSource>();
        }

        public void Use(AbilityUseParams useParams)
        {
            HealSelf(useParams);
            PlayParticleEffect();
        }

        private void PlayParticleEffect()
        {
            var prefab = Instantiate(config.GetParticlePrefab(), transform.position, Quaternion.identity);
            prefab.transform.parent = transform;
            ParticleSystem selfHealParticleSystem = prefab.GetComponent<ParticleSystem>();
            selfHealParticleSystem.Play();
            Destroy(prefab, selfHealParticleSystem.main.duration);
        }

        private void HealSelf(AbilityUseParams useParams)
        {
            player.Heal(config.GetExtraHealth());
            audioSource.clip = config.SpecialEffectClip();
            audioSource.Play();
        }
    }
}
