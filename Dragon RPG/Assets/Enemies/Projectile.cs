using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour {

    
    public float projectileSpeed;
    private float damageCaused;

    public void SetDamage(float damage)
    {
        damageCaused = damage;
    }
    
	void OnTriggerEnter(Collider collider)
    {
        Component damageableComponent = collider.gameObject.GetComponent(typeof(IDamagable));
        if(damageableComponent)
        {
            (damageableComponent as IDamagable).TakeDamage(damageCaused);
        }
    }

}
