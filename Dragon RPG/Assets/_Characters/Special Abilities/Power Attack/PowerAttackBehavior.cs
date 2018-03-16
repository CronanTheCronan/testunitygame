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

        // Update is called once per frame
        void Update()
        {

        }

        public void Use(AbilityUseParams useParams)
        {
            print("Power attack used, extra damage: " + config.GetExtraDamage());
            print("Base damage: " + useParams.baseDamage);
        }
    }

}
