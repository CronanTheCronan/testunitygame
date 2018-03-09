using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BridgeDestroy : MonoBehaviour {

    GameObject giant = null;
	// Use this for initialization
	void Start () {
        giant = GameObject.FindGameObjectWithTag("Giant");
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Giant"))
            Destroy(gameObject);
    }
}
