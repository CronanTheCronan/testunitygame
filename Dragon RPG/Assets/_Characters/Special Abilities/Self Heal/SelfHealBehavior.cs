using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Core;
using System;

namespace RPG.Characters
{
    public class SelfHealBehavior : AbilityBehavior
    {
        Player player;

        void Start()
        {
            player = GetComponent<Player>();
        }

        public override void Use(GameObject target)
        {
            HealSelf(target);
            PlayParticleEffect();
            PlayAbilitySound();
        }

        private void HealSelf(GameObject target)
        {
            var playerHealth = player.GetComponent<Health>();
            playerHealth.Heal((config as SelfHealConfig).GetExtraHealth());
        }
    }
}
