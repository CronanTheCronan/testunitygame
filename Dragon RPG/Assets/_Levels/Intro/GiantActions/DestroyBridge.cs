using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyBridge : MonoBehaviour {

    [SerializeField] int layerFilter = 11;
    [SerializeField] float triggerRadius = 5f;

    GameObject[] leCubes;
    
    void Start()
    {
        SphereCollider sphereCollider = gameObject.AddComponent<SphereCollider>();
        sphereCollider.radius = triggerRadius;
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
        }
    }
}
