using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof (Rigidbody))]
public class BallHelper : MonoBehaviour {

    public Vector3 Target { get; set; }

    [SerializeField]
    private float PunchForce = 3.0f;

    private Rigidbody Rig;

    private Vector3 StartPosition;

    [SerializeField]
    private AudioClip PunchSound;

    void Start ()
    {
        Rig = GetComponent<Rigidbody>();
        StartPosition = transform.position;
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void Punch()
    {
        //print("Ball.position = " + transform.position + " | targetPosition = " + Target);
        Rig.isKinematic = false;
        Rig.AddForce(Target - transform.position, ForceMode.Impulse);
        GameController.GetInstance().PlaySound(PunchSound);
    }

    public void Reset()
    {
        Rig.isKinematic = true;
        Rig.MovePosition(StartPosition);
    }

    public void Friez()
    {
        Rig.isKinematic = true;
    }
}
