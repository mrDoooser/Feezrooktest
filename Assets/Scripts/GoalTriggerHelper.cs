using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalTriggerHelper : MonoBehaviour {

    [SerializeField]
    private bool StopBallAfrerHit;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnTriggerEnter(Collider other)
    {
        BallHelper ball = other.GetComponent<BallHelper>();
        if(ball)
        {
            GameController.GetInstance().Goal();

            if(StopBallAfrerHit)
                ball.Friez();
        }
    }
}
