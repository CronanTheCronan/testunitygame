using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Core;

namespace RPG.Characters
{
    public class AreaEffectBehavior : MonoBehaviour, ISpecialAbility
    {
        AreaEffectConfig config;

        public void SetConfig(AreaEffectConfig configToSet)
        {
            this.config = configToSet;
        }


        // Use this for initialization
        void Start()
        {
            print("Area effect behavior attached to " + gameObject.name);
        }


        public void Use(AbilityUseParams useParams)
        {
            print("Area Effect used by: " + gameObject.name);
            //Static sphere cast for targets
            RaycastHit[] hits = Physics.SphereCastAll(
                transform.position,
                config.GetRadius(),
                Vector3.up,
                config.GetRadius());

            foreach(RaycastHit hit in hits)
            {
                var damageable = hit.collider.gameObject.GetComponent<IDamageable>();
                if (damageable != null && damageable != this.gameObject.GetComponent<IDamageable>())
                {
                    float damageToDeal = useParams.baseDamage + config.GetDamageToEachTarget();
                    damageable.TakeDamage(damageToDeal);
                }
            }
        }
    }

}
