using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    [SerializeField] private GameObject pointBoxParent;
    private int totalBox;
    private int currentBox;

    public static Action onGameWon;

    private void Start()
    {
        totalBox = pointBoxParent.transform.childCount;
        currentBox = totalBox;
    }

    private void OnEnable()
    {
        PlayerController.onPointBoxCollected += ReduceBox;
    }
    private void OnDisable()
    {
        PlayerController.onPointBoxCollected -= ReduceBox;
    }

    private void ReduceBox(int counter)
    {
        currentBox -= counter;
        if(currentBox <= 0)
        {
            Debug.Log("GAME WON");
            onGameWon?.Invoke();
        }
    }

    public int GetCurrentPoint()
    {
        return currentBox;
    }
    public int GetTotalPoint()
    {
        return totalBox;
    }

}
