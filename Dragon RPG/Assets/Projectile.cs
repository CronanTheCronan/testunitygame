using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour {

    public float damageCaused;
    public float projectileSpeed;
    
	void OnTriggerEnter(Collider collider)
    {
        Component damageableComponent = collider.gameObject.GetComponent(typeof(IDamagable));
        if(damageableComponent)
        {
            (damageableComponent as IDamagable).TakeDamage(damageCaused);
        }
    }

}
