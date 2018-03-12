using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyBridge : MonoBehaviour {

    GameObject bridge;
    GameObject giant;

    void Start()
    {
        bridge = GameObject.FindGameObjectWithTag("Bridge");
        giant = GameObject.FindGameObjectWithTag("Giant");
    }

    void OnTriggerEnter(Collider collider)
    {
        Destroy(bridge);
        DestroyObject(giant);
    }

}
