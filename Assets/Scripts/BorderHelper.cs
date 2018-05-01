using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BorderHelper : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnTriggerEnter(Collider other)
    {
        BallHelper ball = other.GetComponent<BallHelper>();
        if (ball)
        {
            GameController.GetInstance().Miss();
        }
    }

}
