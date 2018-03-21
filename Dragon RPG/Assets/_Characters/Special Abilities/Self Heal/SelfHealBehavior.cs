using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Core;
using System;

namespace RPG.Characters
{
    public class SelfHealBehavior : AbilityBehavior
    {
        Player player = null;

        // Use this for initialization
        void Start()
        {
            player = GetComponent<Player>();
        }

        public override void Use(AbilityUseParams useParams)
        {
            HealSelf(useParams);
            PlayParticleEffect();
            PlayAbilitySound();
        }

        private void HealSelf(AbilityUseParams useParams)
        {
            player.Heal((config as SelfHealConfig).GetExtraHealth());
        }
    }
}
