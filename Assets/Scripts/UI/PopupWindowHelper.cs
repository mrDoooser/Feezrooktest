using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopupWindowHelper : MonoBehaviour {

    [SerializeField]
    private RectTransform TargetPosition;

    [SerializeField]
    private float Speed = 2;

    private bool PlayMoving;
    private RectTransform StartPosition;

    // Use this for initialization
    void Start () {
        PlayMoving = false;
        StartPosition = transform.parent as RectTransform;
    }
	
	// Update is called once per frame
	void Update () {

        if(PlayMoving)
        {
            MoveWindow();
        }
		
	}

    public void Show()
    {
        //PlayMoving = true;
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    void MoveWindow()
    {
        RectTransform currTransform = transform.parent as RectTransform;
        currTransform.position = Vector2.Lerp(transform.position, TargetPosition.position, Speed);
    }
}
