using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelTracker : MonoBehaviour
{

    public static LevelTracker Instance {  get; private set; }
    private int pointsTotal = 0;

    void Start()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    public void IncrementPoints(int points)
    {
        pointsTotal += points;
        Debug.Log($"Points: {pointsTotal}");
    }

    public void ResetPoints()
    {
        pointsTotal = 0;
    }
}
