using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHelper : MonoBehaviour {

    public BallHelper Ball { get; set; }

    private Vector3 StartPosition;

    [SerializeField]
    private float Speed = 2.0f;

    [Range(0, 7)]
    [SerializeField]
    private float Accurancy = 2;

    private float MaxDeviation = 10;

    private bool isBallCollided;
    private bool isMooving;

    void Start ()
    {
        StartPosition = transform.position;
        isBallCollided = false;
        isMooving = false;
    }
	
	void Update ()
    {
        if(isMooving)
        {
            transform.position = Vector3.Lerp(transform.position, 
                                new Vector3(Ball.transform.position.x, 
                                            transform.position.y, 
                                            Ball.transform.position.z), 
                                Speed);
                        }
		
	}

    public void PunchBall()
    {
        Vector3 target = Ball.Target;
        float currDeviation = (MaxDeviation - Accurancy) / 10;
        Vector3 newTarget = new Vector3(target.x + Random.Range(-currDeviation / 2, currDeviation / 2),
                                        target.y + Random.Range(-currDeviation / 2, currDeviation / 2),
                                        target.z);

        Ball.Target = newTarget;
        isBallCollided = false;
        isMooving = true;
    }

    void OnTriggerEnter(Collider other)
    {
        if(Ball != null && other.gameObject == Ball.gameObject)
        {
            isBallCollided = true;
            isMooving = false;
            Ball.Punch();

        }
    }

    public void Reset()
    {
        isBallCollided = false;
        isMooving = false;
        transform.position = StartPosition;

    }
}
