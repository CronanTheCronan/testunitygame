using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Characters
{

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

        public void Use(GameObject target)
        {
            behavior.Use(target);
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