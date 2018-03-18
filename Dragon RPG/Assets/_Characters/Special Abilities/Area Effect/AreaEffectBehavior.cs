using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Core;
using System;

namespace RPG.Characters
{
    public class AreaEffectBehavior : MonoBehaviour, ISpecialAbility
    {
        AreaEffectConfig config;
        AudioSource audioSource;

        public void SetConfig(AreaEffectConfig configToSet)
        {
            this.config = configToSet;
        }

        void Start()
        {
            audioSource = GetComponent<AudioSource>();
        }

        public void Use(AbilityUseParams useParams)
        {
            DealRadialDamage(useParams);
            PlayParticleEffect();
            PlayAudio();
        }

        private void PlayParticleEffect()
        {
            var prefab = Instantiate(config.GetParticlePrefab(), transform.position, Quaternion.identity);
            // TODO decide if particle system attaches to player
            ParticleSystem aoeParticleSystem = prefab.GetComponent<ParticleSystem>();
            aoeParticleSystem.Play();
            Destroy(prefab, aoeParticleSystem.main.duration);
        }

        private void DealRadialDamage(AbilityUseParams useParams)
        {
            //Static sphere cast for targets
            RaycastHit[] hits = Physics.SphereCastAll(
                transform.position,
                config.GetRadius(),
                Vector3.up,
                config.GetRadius());

            foreach (RaycastHit hit in hits)
            {
                var damageable = hit.collider.gameObject.GetComponent<IDamageable>();
                bool hitPlayer = hit.collider.gameObject.GetComponent<Player>();
                if (damageable != null && !hitPlayer)
                {
                    float damageToDeal = useParams.baseDamage + config.GetDamageToEachTarget();
                    damageable.TakeDamage(damageToDeal);
                }
            }
        }

        private void PlayAudio()
        {
            audioSource.clip = config.SpecialEffectClip();
            audioSource.Play();
        }
    }

}
