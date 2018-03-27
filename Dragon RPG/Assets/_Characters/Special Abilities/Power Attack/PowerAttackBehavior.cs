using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Characters
{
    public class PowerAttackBehavior : AbilityBehavior
    {

        public override void Use(GameObject target)
        {
            DealDamage(target);
        }

        private void DealDamage(GameObject target)
        {
            // TODO refactor to find enemy.  Perhaps with sphere collider.
            //useParams.target.TakeDamage(damageToDeal);
        }

        
    }

}
