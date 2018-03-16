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

    public abstract class SpecialAbility : ScriptableObject
    {
        [Header("Special Ability General")]
        [SerializeField] float energyCost = 10f;

        protected ISpecialAbility behavior;

        abstract public void AddComponent(GameObject gameObjectToAttachTo);

        public void Use(AbilityUseParams useParams)
        {
            behavior.Use(useParams);
        }

    }

    public interface ISpecialAbility
    {
        void Use(AbilityUseParams useParams);
    }

}