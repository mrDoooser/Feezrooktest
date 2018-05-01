using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreIndicatorHelper : MonoBehaviour {

    [Space]
    [Header("Approaches markers:")]
    [SerializeField]
    //private GameObject[] Approaches;
    private MeshRenderer[] Approaches;

    [Space]
    [Header("Try markers:")]
    [SerializeField]
    private Material GoalMeterial;
    [SerializeField]
    private Material MissMeterial;
    [SerializeField]
    private Material TryMeterial;

    private int ApproachesCounter = 0;

    private int GoalCounter = 0;

    private int MaxApproaches = 5;

    // Use this for initialization
    void Start () {
        MaxApproaches = Approaches.Length;

        Reset();

    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void Reset()
    {
        foreach (var CurrApproache in Approaches)
        {
            CurrApproache.material = TryMeterial;
        }
        ApproachesCounter = 0;
    }

    public void AddScore(bool success)
    {
        if (ApproachesCounter >= MaxApproaches)
        {
            return;
        }

        Approaches[ApproachesCounter].material = success ? GoalMeterial : MissMeterial;

        ApproachesCounter++;

        if (success)
            GoalCounter++;
    }

    public bool CanOneMoreApproache()
    {
        return ApproachesCounter < MaxApproaches;
    }

    public bool IsPlayerWin()
    {
        return GoalCounter == ApproachesCounter;
    }
}
