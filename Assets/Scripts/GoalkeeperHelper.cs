using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalkeeperHelper : MonoBehaviour {

    private Vector3 StartPosition;

    private Vector3 TargetPosition;

    private bool isMoving;

    [SerializeField]
    private Transform LeftBorder;

    [SerializeField]
    private Transform RightBorder;

    [SerializeField]
    private float Speed;

    void Start ()
    {
        StartPosition = transform.position;
        isMoving = false;
    }
	
	void Update ()
    {
        if (!isMoving)
            return;

        transform.position = Vector3.Lerp(transform.position, TargetPosition, Speed);

        if (transform.position.Equals(TargetPosition))
            isMoving = false;

	}

    public void Reset()
    {
        transform.position = StartPosition;
    }

    public void Activate()
    {
        float newXPos = Random.Range(LeftBorder.position.x, RightBorder.position.x);
        TargetPosition = new Vector3(newXPos, transform.position.y, transform.position.z);
        isMoving = true;
    }

}
