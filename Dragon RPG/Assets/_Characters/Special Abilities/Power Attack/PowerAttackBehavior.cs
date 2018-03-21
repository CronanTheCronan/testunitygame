using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Characters
{
    public class PowerAttackBehavior : AbilityBehavior
    {

        public override void Use(AbilityUseParams useParams)
        {
            DealDamage(useParams);
        }

        private void DealDamage(AbilityUseParams useParams)
        {
            // TODO refactor to find enemy.  Perhaps with sphere collider.
            //useParams.target.TakeDamage(damageToDeal);
        }

        
    }

}
