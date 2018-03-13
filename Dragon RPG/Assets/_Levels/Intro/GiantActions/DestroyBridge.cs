using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyBridge : MonoBehaviour {

    [SerializeField] int layerFilter = 11;
    [SerializeField] float triggerRadius = 5f;
    [SerializeField] bool destroyed = false;

    GameObject[] leCubes;
    
    void Start()
    {
        SphereCollider sphereCollider = gameObject.AddComponent<SphereCollider>();
        //sphereCollider.isTrigger = true;
        sphereCollider.radius = triggerRadius;
        
        //sphereCollider.transform.position = new Vector3(10f, 0.04f, 78f); 
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer == layerFilter)
        {
            if (leCubes == null)
                leCubes = GameObject.FindGameObjectsWithTag("leCube");
            Destroy(gameObject);
            foreach (GameObject leCube in leCubes)
            {
                Destroy(leCube);
            }
           
            destroyed = true;
        }
    }

}
