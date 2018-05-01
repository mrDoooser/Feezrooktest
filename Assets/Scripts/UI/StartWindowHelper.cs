using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartWindowHelper : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void StartGame()
    {
        Application.LoadLevel("Main");
    }

    public void Options()
    {

    }

    public void Hiscores()
    {

    }


    public void Exit()
    {
        Application.Quit();
    }

}
