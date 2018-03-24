using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Characters
{
    public struct AbilityUseParams
    {
        public float baseDamage;

        public AbilityUseParams(float baseDamage)
        {
            this.baseDamage = baseDamage;
        }
    }

    public abstract class AbilityConfig : ScriptableObject
    {
        [Header("Special Ability General")]
        [SerializeField] float staminaCost = 10f;
        [SerializeField] GameObject particlePrefab = null;
        [SerializeField] AudioClip specialEffectClip = null;

        protected AbilityBehavior behavior;

        public abstract AbilityBehavior GetBehaviorComponent(GameObject gameObjectToAttachTo);

        public void AddAbilityComponent(GameObject gameObjectToAttachTo)
        {
            AbilityBehavior behaviorComponent = GetBehaviorComponent(gameObjectToAttachTo);
            behaviorComponent.SetConfig(this);
            behavior = behaviorComponent;
        }

        public void Use(AbilityUseParams useParams)
        {
            behavior.Use(useParams);
        }

        public float GetStaminaCost()
        {
            return staminaCost;
        }

        public GameObject GetParticlePrefab()
        {
            return particlePrefab;
        }

        public AudioClip GetAudioClip()
        {
            return specialEffectClip;
        }
    }

}