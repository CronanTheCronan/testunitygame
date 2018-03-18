using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Characters
{
    public class PowerAttackBehavior : MonoBehaviour, ISpecialAbility
    {
        PowerAttackConfig config;

        public void SetConfig(PowerAttackConfig configToSet)
        {
            this.config = configToSet;
        }


        // Use this for initialization
        void Start()
        {
            print("Power Attack behavior attached to " + gameObject.name);
        }

        public void Use(AbilityUseParams useParams)
        {
            print("Power attack used by: " + gameObject.name);
            DealDamage(useParams);
            float damageToDeal = useParams.baseDamage + config.GetExtraDamage();
        }

        private void DealDamage(AbilityUseParams useParams)
        {
            float damageToDeal = useParams.baseDamage + config.GetExtraDamage();
            // TODO refactor to find enemy.  Perhaps with sphere collider.
            //useParams.target.TakeDamage(damageToDeal);
        }

        private void PlayParticleEffect()
        {
            var prefab = Instantiate(config.GetParticlePrefab(), transform.position, Quaternion.identity);
            // TODO decide if particle system attaches to player
            ParticleSystem aoeParticleSystem = prefab.GetComponent<ParticleSystem>();
            aoeParticleSystem.Play();
            Destroy(prefab, aoeParticleSystem.main.duration);
        }

        
    }

}
