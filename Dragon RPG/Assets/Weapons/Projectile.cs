using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour {

    
    public float projectileSpeed;
    float damageCaused;

    public void SetDamage(float damage)
    {
        damageCaused = damage;
    }
    
	void OnCollisionEnter(Collision collision)
    {
        Component damageableComponent = collision.gameObject.GetComponent(typeof(IDamagable));
        if(damageableComponent)
        {
            (damageableComponent as IDamagable).TakeDamage(damageCaused);
        }
    }

}
