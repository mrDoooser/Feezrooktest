using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ETargetingState
{
    NotActive,
    HorizontalTargeting,
    VerticalTargeting,
    TargetResolved
}

public enum E2DDirection
{
    Left,
    Right,
    Up,
    Down
}

public class TargetingMarkerHelper : MonoBehaviour {

    // Описание пространства, доступного для движения маркера прицеливания
    [Space]
    [Header("Targeting borders:")]
    [SerializeField]
    private Transform LeftTargetingBorder;

    [SerializeField]
    private Transform RightTargetingBorder;

    [SerializeField]
    private Transform TopTargetingBorder;

    [SerializeField]
    private Transform DownTargetingBorder;

    [SerializeField]
    private float _crossSpeed;

    public float CrossSpeed
    {
        get { return _crossSpeed; }
        set { _crossSpeed = value; }
    }


    public delegate void TargetingMarkerHendler(Vector3 result);

    public event TargetingMarkerHendler TargetingEnded;

    private ETargetingState TargetingState = ETargetingState.NotActive;

    private E2DDirection CurrentDirection;

    void Start ()
    {

    }
	
	void Update ()
    {
        if (TargetingState == ETargetingState.NotActive 
            || TargetingState == ETargetingState.TargetResolved)
            return;

        if (Input.GetKeyDown(KeyCode.Space))
            ChangeState();

        if (TargetingState == ETargetingState.HorizontalTargeting)
            HorizontalTargeting();

        if (TargetingState == ETargetingState.VerticalTargeting)
            VerticalTargeting();

    }

    public void StartTargeting()
    {
        TargetingState = ETargetingState.NotActive;
        gameObject.SetActive(true);
        transform.position = new Vector3(RightTargetingBorder.position.x, DownTargetingBorder.position.y, RightTargetingBorder.position.z);
        ChangeState();
    }

    void EndTargeting()
    {
        ChangeState();
        TargetingEnded(transform.position);
        gameObject.SetActive(false);
    }

    void HorizontalTargeting()
    {
        int direction = 0;

        // Можем двигаться только влево и вправо. direction опредеяет направление своим знаком
        if (CurrentDirection == E2DDirection.Left)
            direction = -1;
        else if (CurrentDirection == E2DDirection.Right)
            direction = 1;
        else
            return;

        Vector3 shift = new Vector3(CrossSpeed * direction * Time.deltaTime, 0, 0);
        
        transform.position += shift;

        // Достигли левой границы зоны прицеливания - меняем направление
        if(transform.position.x <= LeftTargetingBorder.position.x)
        {
            CurrentDirection = E2DDirection.Right;
            transform.position = new Vector3(LeftTargetingBorder.position.x, transform.position.y, transform.position.z);
        }

        // Достигли правой границы зоны прицеливания - меняем направление
        if (transform.position.x >= RightTargetingBorder.position.x)
        {
            CurrentDirection = E2DDirection.Left;
            transform.position = new Vector3(RightTargetingBorder.position.x, transform.position.y, transform.position.z);
        }

    }

    void VerticalTargeting()
    {
        int direction = 0;

        // Можем двигаться только вверх и вниз. direction опредеяет направление своим знаком
        if (CurrentDirection == E2DDirection.Down)
            direction = -1;
        else if (CurrentDirection == E2DDirection.Up)
            direction = 1;
        else
            return;

        Vector3 shift = new Vector3(0, CrossSpeed * direction * Time.deltaTime, 0);

        transform.position += shift;

        // Достигли нижней границы зоны прицеливания - меняем направление
        if (transform.position.y <= DownTargetingBorder.position.y)
        {
            CurrentDirection = E2DDirection.Up;
            transform.position = new Vector3(transform.position.x, DownTargetingBorder.transform.position.y, transform.position.z);
        }

        // Достигли верхней границы зоны прицеливания - меняем направление
        if (transform.position.y >= TopTargetingBorder.position.y)
        {
            CurrentDirection = E2DDirection.Down;
            transform.position = new Vector3(transform.position.x, TopTargetingBorder.position.y, transform.position.z);
        }
    }

    void ChangeState()
    {
        switch (TargetingState)
        {
            case ETargetingState.NotActive:
                TargetingState = ETargetingState.HorizontalTargeting;
                CurrentDirection = E2DDirection.Left;
                break;

            case ETargetingState.HorizontalTargeting:
                TargetingState = ETargetingState.VerticalTargeting;
                CurrentDirection = E2DDirection.Up;
                break;

            case ETargetingState.VerticalTargeting:
                TargetingState = ETargetingState.TargetResolved;
                EndTargeting();
                break;

            case ETargetingState.TargetResolved:
                TargetingState = ETargetingState.NotActive;
                break;
        }
    }
}
